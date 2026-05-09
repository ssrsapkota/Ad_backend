using Microsoft.EntityFrameworkCore;
using Partpurja.Domain.Models.Users;
using Partpurja.Domain.Models.Vehicle;

namespace Partpurja.Infrastructure.Presistance;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<VehicleInfo> Vehicles => Set<VehicleInfo>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>()
            .HasMany(c => c.Vehicles)
            .WithOne(v => v.Customer)
            .HasForeignKey(v => v.CustomerId);
    }
}