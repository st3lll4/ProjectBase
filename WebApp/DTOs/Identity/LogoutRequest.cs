using System.ComponentModel.DataAnnotations;

namespace WebApp.DTOs.Identity;

public class LogoutRequest
{
        [Required]
        [StringLength(128)]
        public string RefreshToken { get; set; } = default!;
}