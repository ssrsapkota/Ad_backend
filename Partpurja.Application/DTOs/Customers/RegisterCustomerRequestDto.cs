using System.ComponentModel.DataAnnotations;

namespace Partpurja.Application.DTOs.Customers;

public class RegisterCustomerRequestDto
{
    [Required]
    public string FirstName { get; set; } = null!;

    [Required]
    public string LastName { get; set; } = null!;

    [Required]
    [Phone]
    public string PhoneNumber { get; set; } = null!;

    [EmailAddress]
    public string? Email { get; set; }

    public string? Address { get; set; }

    [Required]
    public string VehicleNumber { get; set; } = null!;

    public string? Brand { get; set; } // Changed from Make to Brand
    public string? Model { get; set; }
    [Range(1900, 2100, ErrorMessage = "Please enter a valid year between 1900 and 2100")]
    public int? Year { get; set; }
}