using System.ComponentModel.DataAnnotations;

namespace E_Commerce_Backend.Model
{
    public class ResetPasswordDto
    {
        [Required(ErrorMessage = "Please provide the reset token.")]
        public string guid { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please provide the new password.")]
        public string newPassword { get; set; } = string.Empty;
    }
}
