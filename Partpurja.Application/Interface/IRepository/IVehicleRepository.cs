using Partpurja.Domain.Models.Vehicle;

namespace Partpurja.Application.Interface.IRepository;

public interface IVehicleRepository
{
    Task AddAsync(VehicleInfo vehicle, CancellationToken ct = default);
}