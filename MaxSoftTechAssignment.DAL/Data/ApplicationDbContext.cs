using System.Data;
using MaxSoftTechAssignment.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace MaxSoftTechAssignment.DAL.Data;

public class ApplicationDbContext:IdentityDbContext<User,Role,string>
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Shop> Shops { get; set; }
    public override DbSet<User> Users { get; set; }
    public override DbSet<Role> Roles { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Role>()
            .HasData(
                new Role(Constants.Admin)
                ,new Role(Constants.Manager)
                ,new Role(Constants.Salesman)
                );
        builder.Entity<Product>()
            .HasAlternateKey(entity => 
                new{ entity.Name, entity.ShopId }
            );
        builder.Entity<Product>()
            .Property(p => p.Price)
            .HasColumnType(SqlDbType.Money.ToString());
        
        
        
        // builder.Entity<Role>()
        //     .HasMany(r => r.Users)
        //     .WithMany(u => u.Roles)
        //     .UsingEntity<RoleUser>();
        //
        // builder.Entity<User>()
        //     .HasMany(u => u.Roles)
        //     .WithMany(r => r.Users)
        //     .UsingEntity<RoleUser>();









        base.OnModelCreating(builder);
    }
}