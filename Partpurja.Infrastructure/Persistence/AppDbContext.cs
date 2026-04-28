using Microsoft.EntityFrameworkCore;
using Partpurja.Domain.Models;

namespace Partpurja.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // =========================
        // Identity / Users
        // =========================

        public DbSet<Role> Roles { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<StaffProfile> StaffProfiles { get; set; }


        // =========================
        // Customers & Vehicles
        // =========================

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Vehicle> Vehicles { get; set; }


        // =========================
        // Vendors & Parts
        // =========================

        public DbSet<Vendor> Vendors { get; set; }

        public DbSet<Part> Parts { get; set; }


        // =========================
        // Purchase Invoices
        // =========================

        public DbSet<PurchaseInvoice> PurchaseInvoices { get; set; }

        public DbSet<PurchaseInvoiceItem> PurchaseInvoiceItems { get; set; }


        // =========================
        // Sales Invoices
        // =========================

        public DbSet<SalesInvoice> SalesInvoices { get; set; }

        public DbSet<SalesInvoiceItem> SalesInvoiceItems { get; set; }


        // =========================
        // Appointments
        // =========================

        public DbSet<Appointment> Appointments { get; set; }


        // =========================
        // Requests & Reviews
        // =========================

        public DbSet<UnavailablePartRequest> UnavailablePartRequests { get; set; }

        public DbSet<ServiceReview> ServiceReviews { get; set; }


        // =========================
        // Notifications
        // =========================

        public DbSet<Notification> Notifications { get; set; }


        // =========================
        // Credit Reminders
        // =========================

        public DbSet<CreditReminder> CreditReminders { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            // Role → Users (1:M)
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);


            // User → StaffProfile (1:1)
            modelBuilder.Entity<StaffProfile>()
                .HasOne(sp => sp.User)
                .WithOne(u => u.StaffProfile)
                .HasForeignKey<StaffProfile>(sp => sp.UserId);


            // User → Customer (1:1)
            modelBuilder.Entity<Customer>()
                .HasOne(c => c.User)
                .WithOne(u => u.Customer)
                .HasForeignKey<Customer>(c => c.UserId);


            // Customer → Vehicles (1:M)
            modelBuilder.Entity<Vehicle>()
                .HasOne(v => v.Customer)
                .WithMany(c => c.Vehicles)
                .HasForeignKey(v => v.CustomerId);


            // Vendor → Parts (1:M)
            modelBuilder.Entity<Part>()
                .HasOne(p => p.Vendor)
                .WithMany(v => v.Parts)
                .HasForeignKey(p => p.VendorId);


            // Vendor → PurchaseInvoices (1:M)
            modelBuilder.Entity<PurchaseInvoice>()
                .HasOne(pi => pi.Vendor)
                .WithMany(v => v.PurchaseInvoices)
                .HasForeignKey(pi => pi.VendorId);


            // Customer → SalesInvoices (1:M)
            modelBuilder.Entity<SalesInvoice>()
                .HasOne(si => si.Customer)
                .WithMany(c => c.SalesInvoices)
                .HasForeignKey(si => si.CustomerId);


            // PurchaseInvoice → Items
            modelBuilder.Entity<PurchaseInvoiceItem>()
                .HasOne(pii => pii.PurchaseInvoice)
                .WithMany(pi => pi.Items)
                .HasForeignKey(pii => pii.PurchaseInvoiceId);


            // SalesInvoice → Items
            modelBuilder.Entity<SalesInvoiceItem>()
                .HasOne(sii => sii.SalesInvoice)
                .WithMany(si => si.Items)
                .HasForeignKey(sii => sii.SalesInvoiceId);
        }
    }
}