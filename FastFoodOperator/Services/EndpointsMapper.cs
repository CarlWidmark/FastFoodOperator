using FastFoodOperator.Model;
using Microsoft.EntityFrameworkCore;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace FastFoodOperator.Services;

public static class EndpointsMapper
{
    public static void MapEndpoints(this WebApplication app, List<WebSocket> webSocketConnections)
    {
        app.MapGet("/pizzas", (PizzaShopContext context) =>
            TypedResults.Ok(context.Pizzas
                .Include(p => p.PizzaIngredients)
                .ThenInclude(pi => pi.Ingredient)
                .Select(p => p.ToPizzaDTO())
                .ToList()));

        app.MapGet("/drinks", (PizzaShopContext context) => TypedResults.Ok(context.Drinks));
        app.MapGet("/extras", (PizzaShopContext context) => TypedResults.Ok(context.Extras));

        app.MapPost("/orders", async (PizzaShopContext db, OrderRequest request) =>
        {
            var pizzas = await db.Pizzas
                .Include(p => p.PizzaIngredients).ThenInclude(pi => pi.Ingredient)
                .Where(p => request.PizzaIds.Contains(p.Id)).ToListAsync();

            var drinks = await db.Drinks
                .Where(d => request.DrinkIds.Contains(d.Id)).ToListAsync();

            var extras = await db.Extras
                .Where(e => request.ExtraIds.Contains(e.Id)).ToListAsync();

            var order = new Order
            {
                IsStartedInKitchen = false,
                IsCooked = false,
                IsPickedUp = false,
                EatHere = request.EatHere,
                OrderPizzas = pizzas.Select(p => new OrderPizza { Pizza = p, Quantity = 1 }).ToList(),
                OrderDrinks = drinks.Select(d => new OrderDrink { Drink = d, Quantity = 1 }).ToList(),
                OrderExtras = extras.Select(e => new OrderExtra { Extra = e, Quantity = 1 }).ToList()
            };

            db.Orders.Add(order);
            await db.SaveChangesAsync();

            await BroadcastOrder(order, webSocketConnections); // Skicka den nya ordern till frontend via WebSocket

            return Results.Ok(order.ToCustomerOrder());
        });


        app.MapGet("/orders/allOrders", async (PizzaShopContext db) =>
        {
            var orders = await LoadOrders(db);
            return Results.Ok(orders.Select(o => o.ToOrderDTO()));
        });

        app.MapPut("/orders/{orderId}/IsStartedInKitchen", async (int orderId, PizzaShopContext db) =>
        {
            var order = await db.Orders
                .IncludeAll()
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null) return Results.NotFound("Ordern hittades inte.");

            order.IsStartedInKitchen = true;
            await db.SaveChangesAsync();

            await BroadcastOrder(order, webSocketConnections);
            return Results.Ok(order.ToOrderDTO());
        });

        app.MapPut("/orders/{orderId}/DoneInKitchen", async (int orderId, PizzaShopContext db) =>
        {
            var order = await db.Orders
                .IncludeAll()
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null) return Results.NotFound("Ordern hittades inte.");

            order.IsCooked = true;
            await db.SaveChangesAsync();

            await BroadcastOrder(order, webSocketConnections);
            return Results.Ok(order.ToOrderDTO());
        });

        app.MapPut("/orders/{orderId}/IsCollectedByCustomer", async (int orderId, PizzaShopContext db) =>
        {
            var order = await db.Orders
                .IncludeAll()
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null) return Results.NotFound("Ordern hittades inte.");

            order.IsPickedUp = true;
            await db.SaveChangesAsync();

            await BroadcastOrder(order, webSocketConnections);
            return Results.Ok(order.ToOrderDTO());
        });
        app.MapGet("/pizza/{id}", async (int id, PizzaShopContext context) =>
        {
            var pizza = await context.Pizzas
                .Include(p => p.PizzaIngredients)
                .ThenInclude(pi => pi.Ingredient)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pizza == null)
            {
                return Results.NotFound("Pizza not found.");
            }

            return Results.Ok(pizza.ToPizzaDTO());
        });
        app.MapGet("/orders/{orderId}", async (int orderId, PizzaShopContext db) =>
        {
            // Hämta den specifika ordern med alla relaterade data
            var order = await db.Orders
                .IncludeAll() // Använda din helper-metod för att hämta alla relaterade entiteter
                .FirstOrDefaultAsync(o => o.Id == orderId);

            // Om ordern inte finns, returnera ett 404-fel
            if (order == null)
            {
                return Results.NotFound("Ordern hittades inte.");
            }

            // Om ordern finns, returnera den som en DTO för kunden
            return Results.Ok(order.ToCustomerOrder());
        });

    }

    private static async Task BroadcastOrder(Order order, List<WebSocket> clients)
    {
        var orderDto = order.ToOrderDTO();
        var json = JsonSerializer.Serialize(orderDto);
        var buffer = Encoding.UTF8.GetBytes(json);

        foreach (var socket in clients.ToList())
        {
            if (socket.State == WebSocketState.Open)
            {
                await socket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
    }

    private static async Task<List<Order>> LoadOrders(PizzaShopContext db)
    {
        return await db.Orders.IncludeAll().ToListAsync();
    }

    // Extension för att slippa duplicera Include()
    private static IQueryable<Order> IncludeAll(this DbSet<Order> orders)
    {
        return orders
            .Include(o => o.OrderPizzas).ThenInclude(op => op.Pizza).ThenInclude(p => p.PizzaIngredients).ThenInclude(pi => pi.Ingredient)
            .Include(o => o.OrderDrinks).ThenInclude(od => od.Drink)
            .Include(o => o.OrderExtras).ThenInclude(oe => oe.Extra);
    }

}
