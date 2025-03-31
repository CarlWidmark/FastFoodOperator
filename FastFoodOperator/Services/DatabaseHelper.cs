using FastFoodOperator.Model;

namespace FastFoodOperator.Services
{
    public class DatabaseHelper
    {
        public static void PopulateDatabase(PizzaShopContext db)
        {
            var tomatsås = new Ingredient { Name = "Tomatsås", Price = 5 };
            var ost = new Ingredient { Name = "Ost", Price = 5 };
            var skinka = new Ingredient { Name = "Skinka", Price = 10 };
            var annanas = new Ingredient { Name = "Annanas", Price = 5 };

            db.Ingredients.AddRange(tomatsås, ost, skinka, annanas);
            db.SaveChanges();

            var pizzas = new List<Pizza>
            {
                new Pizza { Name = "Margherita", Price = 110, PizzaIngredients = new List<PizzaIngredient>
                {
                    new PizzaIngredient { Ingredient = tomatsås },
                    new PizzaIngredient { Ingredient = ost },
                }
                },
                new Pizza { Name = "Vesuvio", Price = 125, PizzaIngredients = new List<PizzaIngredient>
                {
                    new PizzaIngredient { Ingredient = tomatsås },
                    new PizzaIngredient { Ingredient = ost },
                    new PizzaIngredient { Ingredient = skinka }
                }
                },
                new Pizza { Name = "Hawaii", Price = 125, PizzaIngredients = new List<PizzaIngredient>
                {
                    new PizzaIngredient { Ingredient = tomatsås },
                    new PizzaIngredient { Ingredient = ost },
                    new PizzaIngredient { Ingredient = skinka },
                    new PizzaIngredient { Ingredient = annanas }

                }
                }
            };
            db.Pizzas.AddRange(pizzas);
            db.SaveChanges();
        }
    }
}




