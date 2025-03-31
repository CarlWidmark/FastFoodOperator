using FastFoodOperator.Model;

namespace FastFoodOperator.Services
{
    public class DatabaseHelper
    {
        public static void PopulateDatabase(PizzaShopContext db, IServiceProvider serviceProvider)
        {
            db.Pizzas.Add(new Pizza { Name = "Margherita" });
            db.Pizzas.Add(new Pizza { Name = "Hawaii" });
            db.Pizzas.Add(new Pizza { Name = "Pepperoni Special" });
            db.Pizzas.Add(new Pizza { Name = "Vegetariana Deluxe" });
            db.Pizzas.Add(new Pizza { Name = "Quattro Formaggi" });
            db.Pizzas.Add(new Pizza { Name = "Bacon Mania" });
            db.Pizzas.Add(new Pizza { Name = "Tonfisk Special" });
            db.Pizzas.Add(new Pizza { Name = "Kebab Pizza" });
            db.Pizzas.Add(new Pizza { Name = "Capricciosa" });
            db.Pizzas.Add(new Pizza { Name = "Marinara" });
            db.Pizzas.Add(new Pizza { Name = "Mexicana" });
            db.Pizzas.Add(new Pizza { Name = "Pollo BBQ" });
            db.Pizzas.Add(new Pizza { Name = "Funghi" });
            db.Pizzas.Add(new Pizza { Name = "Prosciutto" });


            // Ingredients
            var mozzarella = new Ingredient { Name = "Mozzarella" };
            var tomatoSauce = new Ingredient { Name = "Tomatsås" };
            var pepperoni = new Ingredient { Name = "Pepperoni" };
            var mushrooms = new Ingredient { Name = "Champinjoner" };
            var olives = new Ingredient { Name = "Oliver" };
            var ham = new Ingredient { Name = "Skinka" };
            var pineapple = new Ingredient { Name = "Ananas" };
            var cheddar = new Ingredient { Name = "Cheddar" };
            var parmesan = new Ingredient { Name = "Parmesan" };
            var bacon = new Ingredient { Name = "Bacon" };
            var onion = new Ingredient { Name = "Lök" };
            var tuna = new Ingredient { Name = "Tonfisk" };
            var shrimp = new Ingredient { Name = "Räkor" };
            var kebab = new Ingredient { Name = "Kebab" };
            var yogurtSauce = new Ingredient { Name = "Yoghurt sås" };
            var lettuce = new Ingredient { Name = "Sallad" };
            var tomato = new Ingredient { Name = "Tomat" };
            var redOnion = new Ingredient { Name = "Röd lök" };
            var peppers = new Ingredient { Name = "Paprika" };
            var chicken = new Ingredient { Name = "Kyckling" };
            var bbqSauce = new Ingredient { Name = "BBQ sås" };
            var prosciutto = new Ingredient { Name = "Prosciutto" };

            db.AddRange(mozzarella, tomatoSauce, pepperoni, mushrooms, olives, ham, pineapple, cheddar, parmesan, bacon, onion, tuna, shrimp, kebab, yogurtSauce, lettuce, tomato, redOnion, peppers, chicken, bbqSauce, prosciutto);

            // Ingredienser för varje pizza
            AssignIngredients(db, 1, new List<Ingredient> { mozzarella, tomatoSauce });
            AssignIngredients(db, 2, new List<Ingredient> { mozzarella, tomatoSauce, ham, pineapple });
            AssignIngredients(db, 3, new List<Ingredient> { mozzarella, tomatoSauce, pepperoni, mushrooms, olives });
            AssignIngredients(db, 4, new List<Ingredient> { mozzarella, tomatoSauce, mushrooms, olives, onion, parmesan });
            AssignIngredients(db, 5, new List<Ingredient> { mozzarella, cheddar, parmesan });
            AssignIngredients(db, 6, new List<Ingredient> { mozzarella, tomatoSauce, bacon, onion });
            AssignIngredients(db, 7, new List<Ingredient> { mozzarella, tomatoSauce, tuna, shrimp });
            AssignIngredients(db, 8, new List<Ingredient> { mozzarella, kebab, yogurtSauce, lettuce, tomato, redOnion });
            AssignIngredients(db, 9, new List<Ingredient> { mozzarella, tomatoSauce, ham, mushrooms });
            AssignIngredients(db, 10, new List<Ingredient> { mozzarella, tomatoSauce, tuna, shrimp });
            AssignIngredients(db, 11, new List<Ingredient> { mozzarella, tomatoSauce, pepperoni, peppers, onion });
            AssignIngredients(db, 12, new List<Ingredient> { mozzarella, bbqSauce, chicken, onion });
            AssignIngredients(db, 13, new List<Ingredient> { mozzarella, tomatoSauce, mushrooms });
            AssignIngredients(db, 14, new List<Ingredient> { mozzarella, tomatoSauce, prosciutto });



            db.SaveChanges();
        }


        private static void AssignIngredients(PizzaShopContext db, int pizzaId, List<Ingredient> ingredients) // Cambiado a "AssignIngredients"
        {
            var pizza = db.Pizzas.Find(pizzaId);
            if (pizza != null)
            {
                pizza.Ingredients = ingredients;
            }


        }
      
    
    
    }
    }


