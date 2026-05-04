using Partpurja.Domain.Models;

namespace Partpurja.Application.Interface.IRepository
{
    public interface IVehicleRepository
    {
        Task<List<Vehicle>> GetAllAsync();
        Task<Vehicle> CreateAsync(Vehicle vehicle);
    }
}