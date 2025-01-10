using System.ComponentModel.DataAnnotations;

namespace E_Commerce_Backend.Model
{
    public class RemoveCartItemDto
    {
        [Required]
        public int AllProductId { get; set; }
    }
}
