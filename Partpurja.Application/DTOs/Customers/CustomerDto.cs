namespace Partpurja.Application.DTOs.Customers;

public class CustomerDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string? Email { get; set; }
    public List<VehicleDto> Vehicles { get; set; } = new();
}