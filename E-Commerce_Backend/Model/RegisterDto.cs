using System.ComponentModel.DataAnnotations;

namespace E_Commerce_Backend.Model
{
  public class RegisterDto
  {
    [Required]
    [MaxLength(100)]
    public string firstname { get; set; } = string.Empty;
    [Required]
    [MaxLength(100)]
    public string lastname { get; set; } = string.Empty;
    [Required]
    [MaxLength(100)]
    public string email { get; set; } = string.Empty;
    [Required]
    [MaxLength(100)]
    public string phonenumber { get; set; } = string.Empty;
    [Required]
    [MaxLength(100)]
    public string password { get; set; } = string.Empty;

  }
}
