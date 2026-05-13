using Partpurja.Domain.Models.Users;

namespace Partpurja.Domain.Models.Vehicle;

public class VehicleInfo
{
    public int Id { get; set; } // Matches SERIAL in PostgreSQL
    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;
    public string VehicleNumber { get; set; } = null!;
    public string? Brand { get; set; } // Changed from Make to Brand to match SQL
    public string? Model { get; set; }
    public int? Year { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}