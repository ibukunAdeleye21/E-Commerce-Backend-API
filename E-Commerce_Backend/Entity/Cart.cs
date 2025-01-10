using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce_Backend.Entity
{
    public class Cart
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("UserId")]
        public int UserId { get; set; }
        public User? User { get; set; } // One-to-One relationship with User
        public bool IsActive { get; set; }

        public ICollection<CartItem> CartItems { get; set; } 
            = new List<CartItem>(); // One-to-Many relationship with CartItems

        public Order? Order { get; set; } // One-to-One relationship with Order

    }
}
