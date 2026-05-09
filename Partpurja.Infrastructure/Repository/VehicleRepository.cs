using Partpurja.Application.Interface.IRepository;
using Partpurja.Domain.Models.Vehicle;
using Partpurja.Infrastructure.Presistance;

namespace Partpurja.Infrastructure.Repository;

public class VehicleRepository : IVehicleRepository
{
    private readonly AppDbContext _db;
    public VehicleRepository(AppDbContext db) => _db = db;

    public async Task AddAsync(VehicleInfo vehicle, CancellationToken ct = default)
    {
        _db.Vehicles.Add(vehicle);
        await _db.SaveChangesAsync(ct);
    }
}