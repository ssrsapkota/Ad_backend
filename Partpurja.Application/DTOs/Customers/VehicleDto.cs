namespace Partpurja.Application.DTOs.Customers;

public class VehicleDto
{
    public string VehicleNumber { get; set; } = null!;
    public string? Brand { get; set; } // Changed from Make to Brand
    public string? Model { get; set; }
    public int? Year { get; set; }
}
