namespace FastFoodOperator.Services
{
    public class OrderRequest
    {
        public List<int> PizzaIds { get; set; } = new();
        public List<int> DrinkIds { get; set; } = new();
        public List<int> ExtraIds { get; set; } = new();
        public List<MenuRequest> Menus { get; set; } = new();
        public bool EatHere { get; set; }
        public List<int> MenuIds { get; set; } = new();
    }
    public class MenuRequest
    {
        public int PizzaId { get; set; }
        public int DrinkId { get; set; }
        public int ExtraId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Quantity { get; set; } = 1;
    }



}