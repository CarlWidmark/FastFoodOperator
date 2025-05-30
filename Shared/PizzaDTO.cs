﻿namespace Shared
{
    public class PizzaDTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public decimal Price { get; set; }
        public List<string>? Ingredients { get; set; }
        public List<string>? CustomIngredients { get; set; }
    }
    public class CustomerPizzaDTO
    {
        public required string Name { get; set; }
        public List<string>? Ingredients { get; set; }
        public List<string>? CustomIngredients { get; set; }
        public decimal Price { get; set; }
    }
    public class PizzaInKitchenDTO
    {
        public required string Name { get; set; }
        public List<string>? Ingredients { get; set; }
    }
}
