using FastFoodOperator.Model;

namespace FastFoodOperator.Services
{
    public class DatabaseHelper
    {
        public static void PopulateDatabase(PizzaShopContext db, IServiceProvider serviceProvider)
        {
            db.Pizzas.Add(new Pizza
            {
                Name = "Margherita"

            });

            db.Pizzas.Add(new Pizza
            {
                Name = "Hawaii"

            });
            db.SaveChanges();
        }
    }
}
