using FastFoodOperator.Model;

namespace FastFoodOperator.Services
{
    public class DatabaseHelper
    {
        public static void PopulateDatabase(PizzaShopContext db)
        {
            var tomatsås = new Ingredient { Name = "Tomatsås", Price = 5 };
            var ost = new Ingredient { Name = "Ost", Price = 10 };
            var skinka = new Ingredient { Name = "Skinka", Price = 10 };
            var annanas = new Ingredient { Name = "Annanas", Price = 10 };
            var lök = new Ingredient { Name = "Lök", Price = 5 };
            var isbergssallad = new Ingredient { Name = "Isbergssallad", Price = 5 };
            var peperoni = new Ingredient { Name = "Peperoni", Price = 10 };
            var kebab = new Ingredient { Name = "Kebabkött", Price = 15 };
            var kebabsås = new Ingredient { Name = "Kebabsås", Price = 15 };
            var champinjoner = new Ingredient { Name = "Champinjoner", Price = 10 };
            var oliver = new Ingredient { Name = "Oliver", Price = 10 };
            var cheddar = new Ingredient { Name = "Cheddar", Price = 10 };
            var parmesan = new Ingredient { Name = "Parmesan", Price = 10 };
            var bacon = new Ingredient { Name = "Bacon", Price = 10 };
            var tuna = new Ingredient { Name = "Tonfisk", Price = 10 };
            var shrimp = new Ingredient { Name = "Räkor", Price = 10 };
            var yogurtSauce = new Ingredient { Name = "Yoghurt sås", Price = 10 };
            var tomato = new Ingredient { Name = "Tomat", Price = 5 };
            var redOnion = new Ingredient { Name = "Röd lök", Price = 5 };
            var peppers = new Ingredient { Name = "Paprika", Price = 5 };
            var chicken = new Ingredient { Name = "Kyckling", Price = 10 };
            var bbqSauce = new Ingredient { Name = "BBQ sås", Price = 10 };
            var prosciutto = new Ingredient { Name = "Prosciutto", Price = 10 };

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
                    new PizzaIngredient { Ingredient = kebabsås}
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
                   new Pizza { Name = "Vegetariana Deluxe", Price = 115, PizzaIngredients = new List<PizzaIngredient>
                {
                    new PizzaIngredient {Ingredient = tomatsås},
                    new PizzaIngredient { Ingredient = ost },
                    new PizzaIngredient { Ingredient = champinjoner },
                    new PizzaIngredient { Ingredient = lök},
                    new PizzaIngredient {Ingredient = oliver}

                }
                },
                    new Pizza { Name = "Quattro Formaggi", Price = 115, PizzaIngredients = new List<PizzaIngredient>
                {
                    new PizzaIngredient {Ingredient = tomatsås},
                    new PizzaIngredient { Ingredient = ost },
                    new PizzaIngredient { Ingredient = cheddar },
                    new PizzaIngredient { Ingredient = parmesan}
                }
                },
                    new Pizza { Name = "Bacon Mania", Price = 115, PizzaIngredients = new List<PizzaIngredient>
                {
                    new PizzaIngredient {Ingredient = tomatsås},
                    new PizzaIngredient { Ingredient = ost },
                    new PizzaIngredient { Ingredient = bacon },
                    new PizzaIngredient { Ingredient = lök}
                }
                },
                    new Pizza { Name = "Tonfisk Special", Price = 115, PizzaIngredients = new List<PizzaIngredient>
                {
                    new PizzaIngredient {Ingredient = tomatsås},
                    new PizzaIngredient { Ingredient = ost },
                    new PizzaIngredient { Ingredient = tuna },
                    new PizzaIngredient { Ingredient = shrimp}
                }
                },
                    new Pizza { Name = "Capricciosa", Price = 115, PizzaIngredients = new List<PizzaIngredient>
                {
                    new PizzaIngredient {Ingredient = tomatsås},
                    new PizzaIngredient { Ingredient = ost },
                    new PizzaIngredient { Ingredient = skinka },
                    new PizzaIngredient { Ingredient = champinjoner}
                }
                },
            };
            db.Pizzas.AddRange(pizzas);
            db.SaveChanges();

            var drinks = new List<Drink>
             {
                new Drink { Name = "Coca-Cola", Size = 33, Unit = "Cl", Price = 20 },
                new Drink { Name = "Pepsi", Size = 33, Unit = "Cl", Price = 20 },
                new Drink { Name = "Fanta", Size = 33, Unit = "Cl", Price = 20 },
                new Drink { Name = "Sprite", Size = 33, Unit = "Cl", Price = 20 },
                new Drink { Name = "Loka", Size = 33, Unit = "Cl", Price = 20 },
                new Drink { Name = "Ramlösa", Size = 33, Unit = "Cl", Price = 20 },
             };
            db.Drinks.AddRange(drinks);
            db.SaveChanges();

            var extras = new List<Extra>
            {
                new Extra { Name = "Pizzasallad", Info= "Vitkålssallad med vinägrett.", Price= 30},
                new Extra { Name = "Vitlöksbröd", Info= "Bröd med vitlök", Price= 30},
                new Extra { Name = "Mozzarella sticks 4 st", Info= "Friterade mozzarellastavar.", Price= 30},
                new Extra { Name = "Mozzarella sticks 8 st", Info= "Friterade mozzarellastavar.", Price= 60},
                new Extra { Name = "Pommes frites", Info= "Pommes Frittes- tallrik", Price= 45},

            };
            db.Extras.AddRange(extras);
            db.SaveChanges();
        }
    }
}




