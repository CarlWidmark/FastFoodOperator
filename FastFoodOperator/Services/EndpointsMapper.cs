using FastFoodOperator.Model;

namespace FastFoodOperator.Services
{
    public static class EndpointsMapper
    {
        public static void MapEndpoints(this WebApplication app)
        {
            app.MapGet("/", () => "Hello World!");
            app.MapGet("/pizzas", (PizzaShopContext context) => TypedResults.Ok(context.Pizzas.ToList()));
        }
    }
}
