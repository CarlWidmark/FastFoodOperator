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
                     .Select(pi => pi.Ingredient.Name)
                     .ToList(),
                Price = pizza.Price,
            };
        }
        public static OrderDTOs ToOrderDTO(this Order order)
        {
            return new OrderDTOs
            {
                Id = order.Id,
                IsCooked = order.IsCooked,
                IsPickedUp = order.IsPickedUp,
                Pizzas = order.Pizzas.Select(p => p.ToShowKitchenPizzaDTO()).ToList(),
                Drinks = order.Drinks.Select(d => d.ToDrinkDTO()).ToList(),
                Extras = order.Extras.Select(e => e.Name).ToList()
            };

        }
        public static OrderForCustomerDTO ToCustomerOrder(this Order order)
        {
            return new OrderForCustomerDTO
            {
                OrderNr = order.Id,
                Pizzas = (order.Pizzas ?? new List<Pizza>()).Select(p => p.ToCustomerPizzaDTO()).ToList(),
                Drinks = (order.Drinks ?? new List<Drink>()).Select(d => d.ToDrinkDTO()).ToList(),
                Extras = (order.Extras ?? new List<Extra>()).Select(e => e.Name).ToList(),
                TotalPrice = (order.Pizzas ?? new List<Pizza>()).Sum(p => p.Price) +
                     (order.Drinks ?? new List<Drink>()).Sum(d => d.Price) +
                     (order.Extras ?? new List<Extra>()).Sum(e => e.Price)
            };
        }
        public static DrinkDTO ToDrinkDTO(this Drink drink)
        {
            return new DrinkDTO
            {
                Name = drink.Name,
                SizeAndUnit = $"{drink.Size} {drink.Unit}"

            };
        }
        public static PizzaInKitchenDTO ToShowKitchenPizzaDTO(this Pizza pizza)
        {
            return new PizzaInKitchenDTO
            {
                Name = pizza.Name,
                Ingredients = pizza.PizzaIngredients
                     .Select(pi => pi.Ingredient.Name)
                     .ToList()
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
