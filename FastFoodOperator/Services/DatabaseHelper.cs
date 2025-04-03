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
            var lök = new Ingredient { Name = "Lök", Price = 5 };
            var isbergssallad = new Ingredient { Name = "Isbergssallad", Price = 5 };
            var peperoni = new Ingredient { Name = "Peperoni", Price = 10 };
            var kebab = new Ingredient { Name = "Kebabkött", Price = 15 };
            var kebabsås = new Ingredient { Name = "Kebabsås", Price = 15 };

            db.Ingredients.AddRange(tomatsås, ost, skinka, annanas, lök, isbergssallad, peperoni, kebab);
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
                },
                new Pizza { Name = "Kebabpizza", Price = 125, PizzaIngredients = new List<PizzaIngredient>
                {
                    new PizzaIngredient { Ingredient = tomatsås },
                    new PizzaIngredient { Ingredient = ost },
                    new PizzaIngredient { Ingredient = kebab },
                    new PizzaIngredient { Ingredient = isbergssallad },
                    new PizzaIngredient { Ingredient = lök},
                    new PizzaIngredient { Ingredient = kebabsås},


                }
                },
                   new Pizza { Name = "Peperoni", Price = 115, PizzaIngredients = new List<PizzaIngredient>
                {
                    new PizzaIngredient { Ingredient = tomatsås },
                    new PizzaIngredient { Ingredient = ost },
                    new PizzaIngredient { Ingredient = peperoni },
                    new PizzaIngredient { Ingredient = lök}

                }
                },
            };
            db.Pizzas.AddRange(pizzas);
            db.SaveChanges();

            var drinks = new List<Drink>
             {
                new Drink { Name = "Coca-Cola", Size = 33, Unit = "Cl", Price = 20 },
                new Drink { Name = "Pepsi", Size = 50, Unit = "Cl", Price = 25 },
                new Drink { Name = "Fanta", Size = 33, Unit = "Cl", Price = 18 },
                new Drink { Name = "Sprite", Size = 50, Unit = "Cl", Price = 22 },
                new Drink { Name = "Loka", Size = 50, Unit = "Cl", Price = 15 },
                new Drink { Name = "Ramlösa", Size = 50, Unit = "Cl", Price = 15 },
                new Drink { Name = "Apelsinjuice", Size = 25, Unit = "Cl", Price = 28 },
                new Drink { Name = "Äppeljuice", Size = 25, Unit = "Cl", Price = 28 },

             };
            db.Drinks.AddRange(drinks);
            db.SaveChanges();


            var extras = new List<Extra>
            {
                new Extra { Name = "Focaccia", Price = 40 },
                new Extra { Name = "Brödpinnar", Price = 30 },
                new Extra { Name = "Mozzarella Sticks", Price = 65 },
                new Extra { Name = "Vitlöksbröd", Price = 35 },
                new Extra { Name = "Pommes Frites", Price = 25 },
                new Extra { Name = "Caesarsallad", Price = 85 },
                new Extra { Name = "Pizza Salad", Price = 10 },
                new Extra { Name = "Aioli", Price = 15 },
            };
            db.Extras.AddRange(extras);
            db.SaveChanges();
        }
    }
}




