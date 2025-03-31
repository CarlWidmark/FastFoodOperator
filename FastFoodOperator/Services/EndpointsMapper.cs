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
             .ToList()
     )
 );
        }
    }
}
