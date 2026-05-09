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
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.ToTable("customers"); // PostgreSQL table names are usually lowercase by default if not quoted
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("customerid");
            entity.Property(e => e.FirstName).HasColumnName("firstname");
            entity.Property(e => e.LastName).HasColumnName("lastname");
            entity.Property(e => e.PhoneNumber).HasColumnName("phonenumber");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.Address).HasColumnName("address");
            entity.Property(e => e.CreatedAt).HasColumnName("createdat");
        });

        modelBuilder.Entity<VehicleInfo>(entity =>
        {
            entity.ToTable("vehicles");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("vehicleid");
            entity.Property(e => e.CustomerId).HasColumnName("customerid");
            entity.Property(e => e.VehicleNumber).HasColumnName("vehiclenumber");
            entity.Property(e => e.Brand).HasColumnName("brand");
            entity.Property(e => e.Model).HasColumnName("model");
            entity.Property(e => e.Year).HasColumnName("year");
            entity.Property(e => e.CreatedAt).HasColumnName("createdat");

            entity.HasOne(v => v.Customer)
                .WithMany(c => c.Vehicles)
                .HasForeignKey(v => v.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}