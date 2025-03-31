namespace FastFoodOperator.Services
{
    public static class DTOExtentions
    {
        public class OrderRequest
        {
            public List<int> PizzaIds { get; set; } = new();
            public List<int> DrinkIds { get; set; } = new();
            public List<int> ExtraIds { get; set; } = new();
        }


    }
}
