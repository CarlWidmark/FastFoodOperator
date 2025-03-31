using FastFoodOperator.Model;
using Microsoft.EntityFrameworkCore;
namespace FastFoodOperator.Services
{
    public static class EndpointsMapper
    {
        public static void MapEndpoints(this WebApplication app)
        {
            app.MapGet("/", () => "Hello World!");
            app.MapGet("/pizzas", (PizzaShopContext context) => TypedResults.Ok(context.Pizzas.Include(p => p.Ingredients).ToList()));
        }
    }
}
