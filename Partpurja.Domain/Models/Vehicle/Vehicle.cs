using Partpurja.Domain.Models.Users;

namespace Partpurja.Domain.Models.Vehicle;

public class VehicleInfo
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;
    public string VehicleNumber { get; set; } = null!;
    public string? Make { get; set; }
    public string? Model { get; set; }
    public int? Year { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}