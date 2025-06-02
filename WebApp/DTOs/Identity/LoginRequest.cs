using System.ComponentModel.DataAnnotations;

namespace WebApp.DTOs.Identity;

public class LoginRequest 
{
    [Required]
    [StringLength(128)] 
    public string Email { get; set; } = default!;
    
    [Required]
    [StringLength(128)] 
    public string Password { get; set; } = default!;
}