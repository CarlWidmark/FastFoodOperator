using System.Text.Json.Serialization;

namespace Shared
{
    public class OrderDTOs
    {
        public int Id { get; set; }
        public bool IsCooked { get; set; }
        public bool IsPickedUp { get; set; }
        public bool IsStartedInKitchen { get; set; }
        public List<PizzaInKitchenDTO> Pizzas { get; set; } = new();
        public List<DrinkDTO> Drinks { get; set; } = new();
        public List<ExtraDTO> Extras { get; set; } = new();
        public string? Notes { get; set; }
        public bool EatHere { get; set; }

    }
    public class OrderForCustomerDTO
    {
        public int OrderNr { get; set; }
        public DateTime TimeOfOrder { get; set; }
        public List<CustomerPizzaDTO> Pizzas { get; set; } = new();
        public List<DrinkDTO> Drinks { get; set; } = new();
        public List<ExtraDTO> Extras { get; set; } = new();
        public decimal TotalPrice { get; set; }
        public bool EatHere { get; set; }
        public decimal Price { get; set; }

	}
}
