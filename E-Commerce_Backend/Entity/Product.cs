using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace E_Commerce_Backend.Entity
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(300)]
        public string? Description { get; set; }

        public int Price { get; set; }

        public decimal Rating { get; set; }

        [ForeignKey("CategoryId")]
        public int CategoryId { get; set; }

        public Category? Category { get; set; }

        public ICollection<CartItem> CartItems { get; set; } 
            = new List<CartItem>();

        public Product(string name)
        {
            Name = name;
        }
    }
}
