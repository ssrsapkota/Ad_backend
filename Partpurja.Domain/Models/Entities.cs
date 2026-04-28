using System;
using System.Collections.Generic;

namespace Partpurja.Domain.Models
{
    // =========================
    // Enums
    // =========================

    public enum InvoiceStatus
    {
        Pending,
        Paid,
        PartiallyPaid,
        Cancelled
    }

    public enum AppointmentStatus
    {
        Pending,
        Confirmed,
        Completed,
        Cancelled
    }

    public enum RequestStatus
    {
        Pending,
        Approved,
        Ordered,
        Fulfilled,
        Cancelled
    }

    public enum CreditReminderStatus
    {
        Pending,
        Sent,
        Paid,
        Overdue,
        Cancelled
    }

    // =========================
    // Role and User
    // =========================

    public class Role
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty; // Admin, Staff, Customer

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

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

        public DateTime? UpdatedAt { get; set; }

        public StaffProfile? StaffProfile { get; set; }

        public Customer? Customer { get; set; }

        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    }

    // =========================
    // Staff
    // =========================

    public class StaffProfile
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public User? User { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public string Position { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }

    // =========================
    // Customer and Vehicle
    // =========================

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

        public DateTime? UpdatedAt { get; set; }

        public ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();

        public ICollection<SalesInvoice> SalesInvoices { get; set; } = new List<SalesInvoice>();

        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

        public ICollection<ServiceReview> ServiceReviews { get; set; } = new List<ServiceReview>();

        public ICollection<UnavailablePartRequest> UnavailablePartRequests { get; set; } = new List<UnavailablePartRequest>();

        public ICollection<CreditReminder> CreditReminders { get; set; } = new List<CreditReminder>();
    }

    public class Vehicle
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public Customer? Customer { get; set; }

        public string RegistrationNumber { get; set; } = string.Empty;

        public string Brand { get; set; } = string.Empty;

        public string Model { get; set; } = string.Empty;

        public int Year { get; set; }

        public string ChassisNumber { get; set; } = string.Empty;

        public string VehicleCondition { get; set; } = string.Empty; // For AI prediction feature

        public int MonthlyUsageKm { get; set; } // For AI prediction feature

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

        public ICollection<ServiceReview> ServiceReviews { get; set; } = new List<ServiceReview>();
    }

    // =========================
    // Vendor and Part
    // =========================

    public class Vendor
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Contact { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public ICollection<Part> Parts { get; set; } = new List<Part>();

        public ICollection<PurchaseInvoice> PurchaseInvoices { get; set; } = new List<PurchaseInvoice>();
    }

    public class Part
    {
        public int Id { get; set; }

        public string PartNumber { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string Category { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public int Stock { get; set; }

        public int ReorderLevel { get; set; } = 10; // Coursework says notify admin when stock falls below 10

        public int VendorId { get; set; }

        public Vendor? Vendor { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public ICollection<PurchaseInvoiceItem> PurchaseInvoiceItems { get; set; } = new List<PurchaseInvoiceItem>();

        public ICollection<SalesInvoiceItem> SalesInvoiceItems { get; set; } = new List<SalesInvoiceItem>();

        public ICollection<UnavailablePartRequest> UnavailablePartRequests { get; set; } = new List<UnavailablePartRequest>();
    }

    // =========================
    // Purchase Invoice
    // =========================

    public class PurchaseInvoice
    {
        public int Id { get; set; }

        public string InvoiceNumber { get; set; } = string.Empty;

        public int VendorId { get; set; }

        public Vendor? Vendor { get; set; }

        public DateTime Date { get; set; } = DateTime.UtcNow;

        public decimal TotalAmount { get; set; }

        public decimal PaidAmount { get; set; }

        public decimal DiscountAmount { get; set; }

        public InvoiceStatus Status { get; set; } = InvoiceStatus.Pending;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

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

    // =========================
    // Sales Invoice
    // =========================

    public class SalesInvoice
    {
        public int Id { get; set; }

        public string InvoiceNumber { get; set; } = string.Empty;

        public int CustomerId { get; set; }

        public Customer? Customer { get; set; }

        public DateTime Date { get; set; } = DateTime.UtcNow;

        public decimal SubTotal { get; set; }

        public decimal DiscountAmount { get; set; } // 10% discount if purchase is above 5000

        public decimal TotalAmount { get; set; }

        public decimal PaidAmount { get; set; }

        public decimal CreditAmount { get; set; }

        public bool IsInvoiceEmailed { get; set; } = false;

        public InvoiceStatus Status { get; set; } = InvoiceStatus.Pending;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

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

    // =========================
    // Appointment
    // =========================

    public class Appointment
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public Customer? Customer { get; set; }

        public int VehicleId { get; set; }

        public Vehicle? Vehicle { get; set; }

        public int? StaffProfileId { get; set; }

        public StaffProfile? StaffProfile { get; set; }

        public DateTime AppointmentDate { get; set; }

        public string ServiceDescription { get; set; } = string.Empty;

        public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }
    }

    // =========================
    // Unavailable Part Request
    // =========================

    public class UnavailablePartRequest
    {
        public int Id { get; set; }

        public int? PartId { get; set; }

        public Part? Part { get; set; }

        public string RequestedPartName { get; set; } = string.Empty;

        public int Quantity { get; set; }

        public int CustomerId { get; set; }

        public Customer? Customer { get; set; }

        public string Notes { get; set; } = string.Empty;

        public RequestStatus Status { get; set; } = RequestStatus.Pending;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }
    }

    // =========================
    // Service Review
    // =========================

    public class ServiceReview
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public Customer? Customer { get; set; }

        public int VehicleId { get; set; }

        public Vehicle? Vehicle { get; set; }

        public int Rating { get; set; } // 1 to 5

        public string Comment { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }
    }

    // =========================
    // Notification
    // =========================

    public class Notification
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public User? User { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public string Type { get; set; } = string.Empty; // LowStock, CreditReminder, Appointment

        public bool IsRead { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    // =========================
    // Credit Reminder
    // =========================

    public class CreditReminder
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public Customer? Customer { get; set; }

        public int? SalesInvoiceId { get; set; }

        public SalesInvoice? SalesInvoice { get; set; }

        public decimal Amount { get; set; }

        public DateTime DueDate { get; set; }

        public bool IsEmailSent { get; set; } = false;

        public CreditReminderStatus Status { get; set; } = CreditReminderStatus.Pending;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }
    }
}
