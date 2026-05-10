namespace Partpurja.Application.DTOs.Vehicle
{ 
 public class VehicleDto
{
    public int Id { get; set; }

    public int CustomerId { get; set; }

    public string RegistrationNumber { get; set; } = string.Empty;

    public string Brand { get; set; } = string.Empty;

    public string Model { get; set; } = string.Empty;

    public int Year { get; set; }

    public string ChassisNumber { get; set; } = string.Empty;

    public string VehicleCondition { get; set; } = string.Empty;

    public int MonthlyUsageKm { get; set; }
}
}