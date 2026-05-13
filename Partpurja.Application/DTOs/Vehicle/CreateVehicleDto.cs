namespace Partpurja.Application.DTOs.Vehicle
{ 
   public class CreateVehicleDto
{
    // Customer ID to which the vehicle belongs
    public int CustomerId { get; set; }
    
    // Registration number of the vehicle
    public string RegistrationNumber { get; set; } = string.Empty;

    public string Brand { get; set; } = string.Empty;

    public string Model { get; set; } = string.Empty;

    public int Year { get; set; }
    
    // Chassis number of the vehicle
    public string ChassisNumber { get; set; } = string.Empty;
    
    // Condition of the vehicle 
    public string VehicleCondition { get; set; } = string.Empty;

    public int MonthlyUsageKm { get; set; }
}
}