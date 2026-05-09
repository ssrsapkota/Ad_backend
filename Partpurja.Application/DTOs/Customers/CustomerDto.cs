namespace Partpurja.Application.DTOs.Customers;

public class CustomerDto
{
    public int Id { get; set; } // Changed from Guid to int
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string? Email { get; set; }
    public List<VehicleDto> Vehicles { get; set; } = new();
}