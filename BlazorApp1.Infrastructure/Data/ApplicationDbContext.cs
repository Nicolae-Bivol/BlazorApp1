using BlazorApp1.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp1.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
     public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
         : base(options) { }

     public DbSet<Product> Products => Set<Product>();

     protected override void OnModelCreating(ModelBuilder builder)
     {
          base.OnModelCreating(builder);

          // decimal cu precizie corectã pentru SQL Server
          builder.Entity<Product>()
                 .Property(p => p.Price)
                 .HasColumnType("decimal(18,2)");
     }
}
