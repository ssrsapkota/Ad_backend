namespace Partpurja.Application.DTOs.Customers;

public class VehicleDto
{
    public string VehicleNumber { get; set; } = null!;
    public string? Make { get; set; }
    public string? Model { get; set; }
    public int? Year { get; set; }
}
