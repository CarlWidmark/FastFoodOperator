using FastFoodOperator.Model;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace FastFoodOperator.Services
{
    public static class EndpointsMapper
    {
        public static void MapEndpoints(this WebApplication app)
        {
            app.MapGet("/", () => "Hello World!");
            app.MapGet("/pizzas", (PizzaShopContext context) =>
            TypedResults.Ok(
              context.Pizzas
             .Include(p => p.PizzaIngredients)
             .ThenInclude(pi => pi.Ingredient)
             .Select(p => new PizzaDTO
             {
                 Id = p.Id,
                 Name = p.Name,
                 Price = p.Price,
                 Ingredients = p.PizzaIngredients
                     .Select(pi => pi.Ingredient.Name)
                     .ToList()
             })
             .ToList()));
            app.MapGet("/drinks", (PizzaShopContext context) => TypedResults.Ok(context.Drinks));
            app.MapPost("/orders", async (PizzaShopContext db, OrderRequest request) =>
            {
                var pizzasFromDb = await db.Pizzas.Where(p => request.PizzaIds.Contains(p.Id)).ToListAsync();
                var drinksFromDb = await db.Drinks.Where(d => request.DrinkIds.Contains(d.Id)).ToListAsync();
                var extrasFromDb = await db.Extras.Where(e => (request.ExtraIds ?? new()).Contains(e.Id)).ToListAsync();


                var pizzas = request.PizzaIds.SelectMany(id => pizzasFromDb.Where(p => p.Id == id)).ToList();
                var drinks = request.DrinkIds.SelectMany(id => drinksFromDb.Where(d => d.Id == id)).ToList();
                var extras = (request.ExtraIds ?? new()).SelectMany(id => extrasFromDb.Where(e => e.Id == id)).ToList();

                var order = new Order
                {
                    Pizzas = pizzas,
                    Drinks = drinks,
                    Extras = extras,
                    isComplete = false
                };

                db.Orders.Add(order);
                await db.SaveChangesAsync();
                return Results.Ok(order);
            });
            app.MapGet("/orders", async (PizzaShopContext db) =>
            {
                var orders = await db.Orders
                    .Include(o => o.Pizzas)
                    .Include(o => o.Drinks)
                    .Include(o => o.Extras)
                    .ToListAsync();

                return Results.Ok(orders);
            });

        }
    }

}
