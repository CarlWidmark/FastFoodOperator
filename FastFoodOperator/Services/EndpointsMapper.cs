using FastFoodOperator.Model;
using Microsoft.EntityFrameworkCore;

namespace FastFoodOperator.Services
{
    public static class EndpointsMapper
    {
        public static void MapEndpoints(this WebApplication app)
        {
            app.MapGet("/", () => "Hello World!");
            app.MapGet("/pizzas", async (PizzaShopContext context) =>
            {
                var pizzas = await context.Pizzas.ToListAsync();
                return TypedResults.Ok(pizzas);
            });
        }
    }
}
