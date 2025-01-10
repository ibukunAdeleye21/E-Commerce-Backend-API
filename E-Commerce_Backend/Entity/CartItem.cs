using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce_Backend.Entity
{
    public class CartItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("CartId")]
        public int CartId {  get; set; }
        public Cart? Cart { get; set; }

        [ForeignKey("AllProductId")]
        public int AllProductId { get; set; }
        public AllProduct? AllProduct { get; set; }

        [ForeignKey("OrderId")]
        public int? OrderId { get; set; }
        public Order? Order { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal Amount { get; set; }
    }
}
