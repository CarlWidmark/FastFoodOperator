using Microsoft.EntityFrameworkCore;

namespace FastFoodOperator.Model
{
    public class PizzaShopContext : DbContext
    {
        public PizzaShopContext(DbContextOptions<PizzaShopContext> options) : base(options) { }

        public DbSet<Pizza> Pizzas { get; set; }
    }
    public class Pizza
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Price { get; set; }

        public string? Test { get; set; }

        public List<Ingredient> Ingredients { get; set; } = new List<Ingredient>();

    }

}
