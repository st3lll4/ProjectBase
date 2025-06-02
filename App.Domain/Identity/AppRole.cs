using System.ComponentModel.DataAnnotations;
using Base.Domain.Identity;

namespace App.Domain.Identity;

public class AppRole : BaseRole<AppUserRole>
{
    [MaxLength(128)] public string? UserRoleDescription { get; set; }
}