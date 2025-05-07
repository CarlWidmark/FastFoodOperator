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
            
            var pizzaQuantities = request.PizzaIds
                .GroupBy(id => id)
                .ToDictionary(g => g.Key, g => g.Count());

            var pizzas = await db.Pizzas
                .Include(p => p.PizzaIngredients).ThenInclude(pi => pi.Ingredient)
                .Where(p => pizzaQuantities.Keys.Contains(p.Id))
                .ToListAsync();

           
            var drinkQuantities = request.DrinkIds
                .GroupBy(id => id)
                .ToDictionary(g => g.Key, g => g.Count());

            var drinks = await db.Drinks
                .Where(d => drinkQuantities.Keys.Contains(d.Id))
                .ToListAsync();

            
            var extraQuantities = request.ExtraIds
                .GroupBy(id => id)
                .ToDictionary(g => g.Key, g => g.Count());

            var extras = await db.Extras
                .Where(e => extraQuantities.Keys.Contains(e.Id))
                .ToListAsync();

            
            var groupedMenus = request.Menus
                .GroupBy(m => new { m.PizzaId, m.DrinkId, m.ExtraId, m.Name })
                .Select(g => new
                {
                    Key = g.Key,
                    Quantity = g.Sum(m => m.Quantity)
                })
                .ToList();

            
            var menuPizzaIds = groupedMenus.Select(m => m.Key.PizzaId).Distinct();
            var menuDrinkIds = groupedMenus.Select(m => m.Key.DrinkId).Distinct();
            var menuExtraIds = groupedMenus.Select(m => m.Key.ExtraId).Distinct();

            var menuPizzas = await db.Pizzas.Where(p => menuPizzaIds.Contains(p.Id)).ToDictionaryAsync(p => p.Id);
            var menuDrinks = await db.Drinks.Where(d => menuDrinkIds.Contains(d.Id)).ToDictionaryAsync(d => d.Id);
            var menuExtras = await db.Extras.Where(e => menuExtraIds.Contains(e.Id)).ToDictionaryAsync(e => e.Id);

            var orderMenus = groupedMenus.Select(g =>
            {
                var pizza = menuPizzas[g.Key.PizzaId];
                var drink = menuDrinks[g.Key.DrinkId];
                var extra = menuExtras[g.Key.ExtraId];

                var menu = new Menu
                {
                    Name = g.Key.Name,
                    Pizza = pizza,
                    Drink = drink,
                    Extra = extra,
                    Price = pizza.Price + extra.Price 
                };

                return new OrderMenu
                {
                    Menu = menu,
                    Quantity = g.Quantity
                };
            }).ToList();

            
            var order = new Order
            {
                IsStartedInKitchen = false,
                IsCooked = false,
                IsPickedUp = false,
                EatHere = request.EatHere,
                OrderPizzas = pizzas.Select(p => new OrderPizza
                {
                    Pizza = p,
                    Quantity = pizzaQuantities[p.Id]
                }).ToList(),
                OrderDrinks = drinks.Select(d => new OrderDrink
                {
                    Drink = d,
                    Quantity = drinkQuantities[d.Id]
                }).ToList(),
                OrderExtras = extras.Select(e => new OrderExtra
                {
                    Extra = e,
                    Quantity = extraQuantities[e.Id]
                }).ToList(),
                OrderMenus = orderMenus
            };

            
            order.GetTotalPrice();

           
            db.Orders.Add(order);
            await db.SaveChangesAsync();

            await BroadcastOrder(order, webSocketConnections);

            return Results.Ok(order.ToCustomerOrder());
        });


        app.MapGet("/orders/allOrders", async (PizzaShopContext db) =>
        {
            var orders = await db.Orders.IncludeAll().ToListAsync();

            var ordersWithQuantities = orders.Select(o => new
            {
                orderNr = o.Id,
                timeOfOrder = o.TimeOfOrder,
                eatHere = o.EatHere,
                isStartedInKitchen = o.IsStartedInKitchen,
                isCooked = o.IsCooked,
                isPickedUp = o.IsPickedUp,
                pizzas = o.OrderPizzas.Select(op => new {
                    name = op.Pizza.Name,
                    price = op.Pizza.Price,
                    quantity = op.Quantity,
                    ingredients = op.Pizza.PizzaIngredients.Select(pi => pi.Ingredient.Name)
                }),
                drinks = o.OrderDrinks.Select(od => new {
                    name = od.Drink.Name,
                    size = od.Drink.Size,
                    unit = od.Drink.Unit,
                    price = od.Drink.Price,
                    quantity = od.Quantity
                }),
                extras = o.OrderExtras.Select(oe => new {
                    name = oe.Extra.Name,
                    price = oe.Extra.Price,
                    quantity = oe.Quantity
                }),
                orderMenus = o.OrderMenus.Select(om => new {
                    quantity = om.Quantity,
                    menu = new
                    {
                        name = om.Menu.Name,
                        price = om.Menu.Price,
                        pizza = new
                        {
                            name = om.Menu.Pizza.Name,
                            price = om.Menu.Pizza.Price,
                            ingredients = om.Menu.Pizza.PizzaIngredients.Select(pi => pi.Ingredient.Name)
                        },
                        drink = new
                        {
                            name = om.Menu.Drink.Name,
                            size = om.Menu.Drink.Size,
                            unit = om.Menu.Drink.Unit,
                            price = om.Menu.Drink.Price
                        },
                        extra = new
                        {
                            name = om.Menu.Extra.Name,
                            price = om.Menu.Extra.Price
                        }
                    },
                    
                })
                
              
            }).ToList();

            return Results.Ok(ordersWithQuantities);
        });
        app.MapGet("/api/tax/vat/{totalPrice:decimal}", (decimal totalPrice) =>
        {
            var vat = TaxCalculator.CalculateVAT(totalPrice);
            return Results.Ok(vat);
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
            return Results.Ok(order.ToCustomerOrder());
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
            return Results.Ok(order.ToCustomerOrder());
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
            var order = await db.Orders
                .IncludeAll()
                .FirstOrDefaultAsync(o => o.Id == orderId);
            if (order == null)
            {
                return Results.NotFound("Ordern hittades inte.");
            }
            return Results.Ok(order.ToCustomerOrder());
        });
        app.MapGet("/ingredients", (PizzaShopContext context) =>
            TypedResults.Ok(context.Ingredients.Select(i => i.ToIngredientDto()).ToList()));
        app.MapGet("/ingredients/{id}", async (int id, PizzaShopContext context) =>
        {
            var ingredient = await context.Ingredients
                .FirstOrDefaultAsync(i => i.Id == id);

            if (ingredient == null)
            {
                return Results.NotFound("Ingredient not found.");
            }

            return Results.Ok(ingredient.ToIngredientDto());
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

    private static IQueryable<Order> IncludeAll(this DbSet<Order> orders)
    {
        return orders
            .Include(o => o.OrderPizzas).ThenInclude(op => op.Pizza).ThenInclude(p => p.PizzaIngredients).ThenInclude(pi => pi.Ingredient)
            .Include(o => o.OrderDrinks).ThenInclude(od => od.Drink)
            .Include(o => o.OrderExtras).ThenInclude(oe => oe.Extra)
            .Include(o => o.OrderMenus).ThenInclude(om => om.Menu).ThenInclude(m => m.Pizza).ThenInclude(p => p.PizzaIngredients).ThenInclude(pi => pi.Ingredient)
            .Include(o => o.OrderMenus).ThenInclude(om => om.Menu).ThenInclude(m => m.Drink)
            .Include(o => o.OrderMenus).ThenInclude(om => om.Menu).ThenInclude(m => m.Extra);
    }


}
