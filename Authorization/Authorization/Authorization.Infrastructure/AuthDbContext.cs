namespace Authorization.Infrastructure;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models;

public class AuthDbContext : IdentityDbContext<ApplicationUser>
{
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    // public AuthDbContext()
    // {
    //
    // }

    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=172.17.0.1,1433;Database=UntitledArticlesAuthenticationDB;User=sa;Password=Str0ngPa$$w0rd;Trust Server Certificate=true");
    }
}
