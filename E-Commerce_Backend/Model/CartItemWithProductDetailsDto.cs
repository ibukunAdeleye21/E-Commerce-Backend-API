namespace E_Commerce_Backend.Model
{
    public class CartItemWithProductDetailsDto
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; } 
        public decimal Amount { get; set; }
        public int ProductId {  get; set; }
        public string ProductTitle { get; set; } = string.Empty;
        public string ProductDescription { get; set; } = string.Empty;
        public string ProductCategory { get; set; } = string.Empty;
        public string ProductImage { get; set; } = string.Empty;
        public decimal ProductRate { get; set; }  
        public int ProductCount { get; set; }

    }
}
