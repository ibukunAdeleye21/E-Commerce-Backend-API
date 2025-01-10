using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce_Backend.Entity
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Firstname { get; set; } = string.Empty;
        [Required]
        [MaxLength(100)]
        public string Lastname { get; set; } = string.Empty;
        [Required]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;
        [Required]
        [MaxLength(100)]
        public string Phonenumber { get; set; } = string.Empty;
        [Required]
        [MaxLength(100)]
        public string Password { get; set; } = string.Empty;
        public string PasswordResetGuid { get; set; } = string.Empty;
        public DateTime? PasswordResetGuidCreate { get; set; }
        public DateTime? PasswordResetGuidExpiry { get; set; }
        public bool IsUsed { get; set; }

        // One-to-One relationship
        public Cart? Cart { get; set; }

        // One-to-Many relationship
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
  
}


