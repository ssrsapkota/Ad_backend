using Partpurja.Application.DTOs.Vehicle;

namespace Partpurja.Application.Interface.IServices
{
    public interface IVehicleService
    {
        Task<List<VehicleDto>> GetAllAsync();
        Task<VehicleDto?> GetByIdAsync(int id);
        Task<List<VehicleDto>> GetByCustomerIdAsync(int customerId);
        Task<VehicleDto> CreateAsync(CreateVehicleDto dto);
        Task<VehicleDto?> UpdateAsync(int id, UpdateVehicleDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
