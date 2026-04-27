using System;
using System.Collections.Generic;

namespace Partpurja.Domain.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<User> Users { get; set; } = new List<User>();
    }

    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public int RoleId { get; set; }
        public Role? Role { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public StaffProfile? StaffProfile { get; set; }
        public Customer? Customer { get; set; }
    }

    public class StaffProfile
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public string Position { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public class Customer
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
    }

    public class Vehicle
    {
        public int Id { get; set; }
        public string RegistrationNumber { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public class Vendor
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Contact { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public class Part
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public class PurchaseInvoice
    {
        public int Id { get; set; }
        public int VendorId { get; set; }
        public Vendor? Vendor { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<PurchaseInvoiceItem> Items { get; set; } = new List<PurchaseInvoiceItem>();
    }

    public class PurchaseInvoiceItem
    {
        public int Id { get; set; }
        public int PurchaseInvoiceId { get; set; }
        public PurchaseInvoice? PurchaseInvoice { get; set; }
        public int PartId { get; set; }
        public Part? Part { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }

    public class SalesInvoice
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<SalesInvoiceItem> Items { get; set; } = new List<SalesInvoiceItem>();
    }

    public class SalesInvoiceItem
    {
        public int Id { get; set; }
        public int SalesInvoiceId { get; set; }
        public SalesInvoice? SalesInvoice { get; set; }
        public int PartId { get; set; }
        public Part? Part { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }

    public class Appointment
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public int VehicleId { get; set; }
        public Vehicle? Vehicle { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public class UnavailablePartRequest
    {
        public int Id { get; set; }
        public int PartId { get; set; }
        public Part? Part { get; set; }
        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public class ServiceReview
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public int VehicleId { get; set; }
        public Vehicle? Vehicle { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public class Notification
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public string Message { get; set; } = string.Empty;
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public class CreditReminder
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public decimal Amount { get; set; }
        public DateTime DueDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}