using Microsoft.EntityFrameworkCore;

namespace FastFoodOperator.Model
{
    public class PizzaShopContext : DbContext
    {
        public PizzaShopContext(DbContextOptions<PizzaShopContext> options) : base(options) { }

        public DbSet<Pizza> Pizzas { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<PizzaIngredient> PizzaIngredients { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Drink> Drinks { get; set; }
        public DbSet<Extra> Extras { get; set; }

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
    public class Drink
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Unit { get; set; }
        public required decimal Size { get; set; }

        public decimal Price { get; set; }

    }
    public class Extra
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public decimal Price { get; set; }

    }
    public class Order
    {
        public int Id { get; set; }
        public List<Pizza>? Pizzas { get; set; }
        public List<Drink>? Drinks { get; set; }
        public List<Extra>? Extras { get; set; }
        public bool IsCooked { get; set; }
        public bool IsPickedUp { get; set; }


    }
}
