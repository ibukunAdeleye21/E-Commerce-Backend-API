namespace E_Commerce_Backend.Model
{
    public class OrderWithCartItemDto
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty;

        // List of cart items in the order
        public ICollection<CartItemWithProductDetailsDto> CartItems { get; set; } 
            = new List<CartItemWithProductDetailsDto>();
    }
}
