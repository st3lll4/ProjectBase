using App.Domain;
using App.Domain.Identity;
using Base.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace App.DAL.EF;

public class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid, IdentityUserClaim<Guid>, AppUserRole,
    IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
{
    public AppDbContext(DbContextOptions<AppDbContext> options, IUsernameContextHelper userNameContextHelper,
        ILogger<AppDbContext> logger)
        : base(options)
    {
        _userNameContextHelper = userNameContextHelper;
        _logger = logger;
    }
    
    public DbSet<Artist> Artists { get; set; } = default!;

    public DbSet<AppRefreshToken> AppRefreshTokens { get; set; } = default!;

    private readonly IUsernameContextHelper _userNameContextHelper;
    private readonly ILogger<AppDbContext> _logger;


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        /*modelBuilder.Entity<CardPrice>()
            .Property(cp => cp.Currency)
            .HasConversion<string>();*/

        modelBuilder.Entity<AppUserRole>()
            .HasOne(a => a.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(a => a.UserId);

        modelBuilder.Entity<AppUserRole>()
            .HasOne(a => a.Role)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(a => a.RoleId);
    }
}