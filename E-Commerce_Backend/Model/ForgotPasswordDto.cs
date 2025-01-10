using System.ComponentModel.DataAnnotations;

namespace E_Commerce_Backend.Model
{
    public class ForgotPasswordDto
    {
        [Required(ErrorMessage = "Please provide a valid email...")]
        [EmailAddress] // To verify the email address
        public string email { get; set; } = string.Empty;
    }
}
