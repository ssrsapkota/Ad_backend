using Partpurja.Domain.Models.Vehicle;

namespace Partpurja.Domain.Models.Users;

public class Customer : User
{
    public string? Address { get; set; }
    public ICollection<VehicleInfo> Vehicles { get; set; } = new List<VehicleInfo>();
}