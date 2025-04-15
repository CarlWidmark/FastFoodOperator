namespace Shared
{
    public class DrinkDTO
    {
        public string Name { get; set; } = string.Empty;
        public decimal? Size { get; set; }
        public string? Unit { get; set; }
        public decimal? Price { get; set; }
    }
}
