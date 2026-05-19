using Partpurja.Domain.Models;

namespace Partpurja.Application.Interface.IRepository
{
    public interface IVehicleRepository
    {
        Task<List<Vehicle>> GetAllAsync();
        Task<Vehicle?> GetByIdAsync(int id);
        Task<List<Vehicle>> GetByCustomerIdAsync(int customerId);
        Task<Vehicle> CreateAsync(Vehicle vehicle);
        Task<Vehicle?> UpdateAsync(int id, Vehicle vehicle);
        Task<bool> DeleteAsync(int id);
    }
}
