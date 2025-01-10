using System.ComponentModel.DataAnnotations;

namespace E_Commerce_Backend.Model
{
    public class ChangePasswordDto
    {
        [Required]
        public string OldPassword { get; set; } = string.Empty;
        [Required]
        public string NewPassword { get; set; } = string.Empty;
    }
}
