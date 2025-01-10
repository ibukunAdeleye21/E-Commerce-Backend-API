namespace E_Commerce_Backend.Model
{
    public class OrderDto
    {
        public List<CartItemDto> CartItems { get; set; } = new List<CartItemDto>();
        public decimal TotalAmount { get; set; }

    }
}
