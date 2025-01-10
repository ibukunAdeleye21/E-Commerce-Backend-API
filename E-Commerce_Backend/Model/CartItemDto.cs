using System.ComponentModel.DataAnnotations;

namespace E_Commerce_Backend.Model
{
    public class CartItemDto
    {
        [Required]
        public int AllProductId { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int Quantity { get; set; }
        public decimal Amount { get; set; }
    }
}
