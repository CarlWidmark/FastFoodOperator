using FastFoodOperator.Model;
using Microsoft.EntityFrameworkCore;

namespace FastFoodOperator.Services
{
    public static class EndpointsMapper
    {
        public static void MapEndpoints(this WebApplication app)
        {
            app.MapGet("/pizzas", (PizzaShopContext context) =>
            TypedResults.Ok(
              context.Pizzas
             .Include(p => p.PizzaIngredients)
             .ThenInclude(pi => pi.Ingredient)
             .Select(p => p.ToPizzaDTO())
             .ToList()));
            app.MapGet("/drinks", (PizzaShopContext context) => TypedResults.Ok(context.Drinks));
            app.MapPost("/orders", async (PizzaShopContext db, OrderRequest request) =>
            {
                var pizzasFromDb = await db.Pizzas
                .Include(p => p.PizzaIngredients)
                .ThenInclude(pi => pi.Ingredient)
                .Where(p => request.PizzaIds.Contains(p.Id)).ToListAsync();

                var drinksFromDb = await db.Drinks
                .Where(d => request.DrinkIds
                .Contains(d.Id))
                .ToListAsync();

                var extrasFromDb = await db.Extras
                .Where(e => (request.ExtraIds ?? new())
                .Contains(e.Id))
                .ToListAsync();


                var pizzas = request.PizzaIds.SelectMany(id => pizzasFromDb.Where(p => p.Id == id)).ToList();
                var drinks = request.DrinkIds.SelectMany(id => drinksFromDb.Where(d => d.Id == id)).ToList();
                var extras = (request.ExtraIds ?? new()).SelectMany(id => extrasFromDb.Where(e => e.Id == id)).ToList();

                var order = new Order
                {
                    Pizzas = pizzas,
                    Drinks = drinks,
                    Extras = extras,
                    IsCooked = false,
                    IsPickedUp = false
                };

                db.Orders.Add(order);
                await db.SaveChangesAsync();
                return Results.Ok(order.ToCustomerOrder());
            });
            app.MapGet("/orders", async (PizzaShopContext db) =>
            {
                var orders = await db.Orders
                     .Where(o => !(o.IsCooked && o.IsPickedUp))
                     .Include(o => o.Pizzas)
                     .ThenInclude(p => p.PizzaIngredients)
                     .ThenInclude(pi => pi.Ingredient)
                     .Include(o => o.Drinks)
                     .Include(o => o.Extras)
                     .ToListAsync();

                var ordersDto = orders.Select(o => o.ToOrderDTO()).ToList();

                return Results.Ok(ordersDto);
            });
            app.MapPut("/orders/{orderId}/DoneInKitchen", async (int orderId, PizzaShopContext db) =>
            {
                var order = await db.Orders.FindAsync(orderId);
                if (order == null)
                {
                    return Results.NotFound("Ordern hittades inte.");
                }
                order.IsCooked = true;
                await db.SaveChangesAsync();
                return Results.Ok(order);
            });
            app.MapPut("/orders/{orderId}/IsCollectedByCustomer", async (int orderId, PizzaShopContext db) =>
            {
                var order = await db.Orders.FindAsync(orderId);
                if (order == null)
                {
                    return Results.NotFound("Ordern hittades inte.");
                }
                order.IsPickedUp = true;
                await db.SaveChangesAsync();
                return Results.Ok(order);
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

            app.MapGet("/receipt/{orderId}", async (int orderId, PizzaShopContext db) =>
            {
                var order = await db.Orders
                    .Include(o => o.Pizzas)
                    .ThenInclude(p => p.PizzaIngredients)
                    .ThenInclude(pi => pi.Ingredient)
                    .Include(o => o.Drinks)
                    .Include(o => o.Extras)
                    .FirstOrDefaultAsync(o => o.Id == orderId);

                if (order == null)
                {
                    return Results.NotFound("Order not found.");
                }

                var receipt = new ReceiptDTO
                {
                    OrderNumber = order.Id,
                    TotalPrice = (order.Pizzas ?? new List<Pizza>()).Sum(p => p.Price) +
                                 (order.Drinks ?? new List<Drink>()).Sum(d => d.Price) +
                                 (order.Extras ?? new List<Extra>()).Sum(e => e.Price)
                };

               
                foreach (var pizza in order.Pizzas ?? new List<Pizza>())
                {
                    receipt.Items.Add(new ReceiptDTO.ReceiptItemDTO
                    {
                        Name = pizza.Name,
                        Price = pizza.Price,
                        Description = string.Join(", ", pizza.PizzaIngredients.Select(pi => pi.Ingredient.Name))
                    });
                }

               
                foreach (var drink in order.Drinks ?? new List<Drink>())
                {
                    receipt.Items.Add(new ReceiptDTO.ReceiptItemDTO
                    {
                        Name = drink.Name,
                        Price = drink.Price,
                        Description = $"{drink.Size} {drink.Unit}"
                    });
                }

                
                foreach (var extra in order.Extras ?? new List<Extra>())
                {
                    receipt.Items.Add(new ReceiptDTO.ReceiptItemDTO
                    {
                        Name = extra.Name,
                        Price = extra.Price,
                        Description = "Extra topping"
                    });
                }

                return Results.Ok(receipt);
            });
        }
    }
}











        
    


