using FastFoodOperator.Model;
using Shared;

namespace FastFoodOperator.Services
{
    public static class DTOExtentions
    {
        public static PizzaDTO ToPizzaDTO(this Pizza pizza)
        {
            return new PizzaDTO
            {
                Id = pizza.Id,
                Name = pizza.Name,
                Price = pizza.Price,
                Ingredients = pizza.PizzaIngredients
                    .Where(pi => pi.Ingredient != null)
                    .Select(pi => pi.Ingredient.Name)
                    .ToList(),
                CustomIngredients = pizza.PizzaIngredients
                    .Where(pi => pi.Ingredient != null && pi.Ingredient.Price > 0)
                    .Select(pi => pi.Ingredient.Name)
                    .ToList()
            };
        }

        public static CustomerPizzaDTO ToCustomerPizzaDTO(this Pizza pizza)
        {
            return new CustomerPizzaDTO
            {
                Name = pizza.Name,
                Ingredients = pizza.PizzaIngredients
                    .Where(pi => pi.Ingredient != null)
                    .Select(pi => pi.Ingredient.Name)
                    .ToList(),
                Price = pizza.Price,
                CustomIngredients = pizza.PizzaIngredients
                    .Where(pi => pi.Ingredient != null && pi.Ingredient.Price > 0)
                    .Select(pi => pi.Ingredient.Name)
                    .ToList()
            };
        }

        public static OrderDTOs ToOrderDTO(this Order order)
        {
            return new OrderDTOs
            {
                Id = order.Id,
                IsStartedInKitchen = order.IsStartedInKitchen,
                IsCooked = order.IsCooked,
                IsPickedUp = order.IsPickedUp,
                Pizzas = order.OrderPizzas.Select(op => op.Pizza.ToShowKitchenPizzaDTO()).ToList(),
                Drinks = order.OrderDrinks.Select(od => od.Drink.ToDrinkDTO()).ToList(),
                Extras = order.OrderExtras.Select(oe => oe.Extra.ToExtraDTO()).ToList(),
                Notes = order.Notes,
                EatHere = order.EatHere
            };
        }

        public static OrderForCustomerDTO ToCustomerOrder(this Order order)
        {
            return new OrderForCustomerDTO
            {
                OrderNr = order.Id,
                Pizzas = (order.OrderPizzas ?? new List<OrderPizza>())
                    .Select(op => op.Pizza.ToCustomerPizzaDTO())
                    .ToList(),
                Drinks = (order.OrderDrinks ?? new List<OrderDrink>())
                    .Select(od => od.Drink.ToDrinkDTO())
                    .ToList(),
                Extras = (order.OrderExtras ?? new List<OrderExtra>())
                    .Select(oe => oe.Extra.ToExtraDTO())
                    .ToList(),
                TotalPrice = (order.OrderPizzas ?? new List<OrderPizza>())
                    .Sum(op => op.Pizza.Price * op.Quantity) +
                    (order.OrderDrinks ?? new List<OrderDrink>())
                    .Sum(od => od.Drink.Price * od.Quantity) +
                    (order.OrderExtras ?? new List<OrderExtra>())
                    .Sum(oe => oe.Extra.Price * oe.Quantity),
                EatHere = order.EatHere
            };
        }

        public static DrinkDTO ToDrinkDTO(this Drink drink)
        {
            return new DrinkDTO
            {
                Name = drink.Name,
                Size = drink.Size,
                Unit = drink.Unit,
                Price = drink.Price
            };
        }
        public static ExtraDTO ToExtraDTO(this Extra extra)
        {
            return new ExtraDTO
            {
                Name = extra.Name,
                Info = extra.Info,
                Price = extra.Price
            };
        }
        public static PizzaInKitchenDTO ToShowKitchenPizzaDTO(this Pizza pizza)
        {
            return new PizzaInKitchenDTO
            {
                Name = pizza.Name,
                Ingredients = pizza.PizzaIngredients
                     .Where(pi => pi.Ingredient != null)
                     .Select(pi => pi.Ingredient.Name)
                     .ToList()
            };
        }

        public static IngredientDTO ToIngredientDto(this Ingredient ingredient)
        {
            return new IngredientDTO()
            {
                Id = ingredient.Id,
                Name = ingredient.Name,
                Price = ingredient.Price
            };
        }



        public class OrderRequest
        {
            public List<int> PizzaIds { get; set; } = new();
            public List<int> DrinkIds { get; set; } = new();
            public List<int> ExtraIds { get; set; } = new();
        }
    }
}
