using System.ComponentModel.DataAnnotations;
using Base.Domain.Identity;

namespace App.Domain.Identity;


public class AppUser : BaseUser<AppUserRole>
{ 
    [MaxLength(64)] public string FirstName { get; set; } = default!; 
    [MaxLength(64)] public string LastName { get; set; } = default!;

    public ICollection<AppRefreshToken>? RefreshTokens { get; set; }
    
}