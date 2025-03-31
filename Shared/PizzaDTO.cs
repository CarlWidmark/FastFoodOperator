namespace Shared
{
    public class PizzaDTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public decimal Price { get; set; }
        public List<string>? Ingredients { get; set; }
    }
}
