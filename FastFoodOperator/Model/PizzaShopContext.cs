using Microsoft.EntityFrameworkCore;

namespace FastFoodOperator.Model
{
    public class PizzaShopContext : DbContext
    {
        public PizzaShopContext(DbContextOptions<PizzaShopContext> options) : base(options) { }

        public DbSet<Pizza> Pizzas { get; set; }
        public DbSet<Drink> Drinks { get; set; }
        public DbSet<Extra> Extras { get; set; }

        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<PizzaIngredient> PizzaIngredients { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderPizza> OrderPizzas { get; set; }
        public DbSet<OrderDrink> OrderDrinks { get; set; }
        public DbSet<OrderExtra> OrderExtras { get; set; }
        public DbSet<CustomPizzaIngredient> CustomPizzaIngredients { get; set; }

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
        public ICollection<PizzaIngredient> PizzaIngredients { get; set; } = new List<PizzaIngredient>();
    }

    public class Ingredient
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public decimal Price { get; set; }
        public ICollection<PizzaIngredient>? PizzaIngredients { get; set; } = new List<PizzaIngredient>();
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
        public required string Info { get; set; }
        public decimal Price { get; set; }
        public bool IsOptional { get; set; }

    public class OrderPizza
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order? Order { get; set; }
        public int PizzaId { get; set; }
        public Pizza? Pizza { get; set; }
        public int Quantity { get; set; }
        public string? Notes { get; set; }
        public List<CustomPizzaIngredient> CustomIngredients { get; set; } = new();
    }
    public class OrderDrink
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order? Order { get; set; }
        public int DrinkId { get; set; }
        public Drink? Drink { get; set; }
        public int Quantity { get; set; }
    }
    public class CustomPizzaIngredient
    {
        public int Id { get; set; }
        public int OrderPizzaId { get; set; }
        public OrderPizza? OrderPizza { get; set; }
        public int IngredientId { get; set; }
        public Ingredient? Ingredient { get; set; }
        public bool IsAdded { get; set; }
    }
    public class OrderExtra
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order? Order { get; set; }
        public int ExtraId { get; set; }
        public Extra? Extra { get; set; }
        public int Quantity { get; set; }
    }
    public class Order
    {
        public int Id { get; set; }
        public DateTime TimeOfOrder { get; set; } = DateTime.Now;
        public List<OrderPizza> OrderPizzas { get; set; } = new();
        public List<OrderDrink> OrderDrinks { get; set; } = new();
        public List<OrderExtra> OrderExtras { get; set; } = new();
        public bool IsCooked { get; set; }
        public bool IsPickedUp { get; set; }
        public bool IsStartedInKitchen { get; set; }
        public string? Notes { get; set; }
        public bool EatHere { get; set; }
    }

}
