using Microsoft.EntityFrameworkCore;

namespace FastFoodOperator.Model
{
    public class PizzaShopContext : DbContext
    {
        public PizzaShopContext(DbContextOptions<PizzaShopContext> options) : base(options) { }

        public DbSet<Pizza> Pizzas { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<PizzaIngredient> PizzaIngredients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PizzaIngredient>()
                .HasKey(pi => new { pi.PizzaId, pi.IngredientId });
        }
    }


    public class Pizza
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public decimal Price { get; set; }
        public required ICollection<PizzaIngredient> PizzaIngredients { get; set; }

    }
    public class Ingredient
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public decimal Price { get; set; }
        public ICollection<PizzaIngredient>? PizzaIngredients { get; set; }

    }
    public class PizzaIngredient
    {
        public int PizzaId { get; set; }
        public Pizza? Pizza { get; set; }

        public int IngredientId { get; set; }
        public Ingredient? Ingredient { get; set; }
    }
}
