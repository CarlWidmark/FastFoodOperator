namespace FastFoodOperator.Model
{
    public class ReceiptDTO
    {
        public string Title { get; set; } = "Kvitto"; 
        public int OrderNumber { get; set; } 
        public List<ReceiptItemDTO> Items { get; set; } = new();
        public decimal TotalPrice { get; set; }

        public class ReceiptItemDTO
        {
            public string? Name { get; set; } 
            public decimal Price { get; set; } 
            public string ?Description { get; set; }
        }
    }
}

