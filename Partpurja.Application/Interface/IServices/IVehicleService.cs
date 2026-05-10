using Partpurja.Application.DTOs.Vehicle;

namespace Partpurja.Application.Interface.IServices
{
    public interface IVehicleService
    {
        Task<List<VehicleDto>> GetAllAsync();
        Task<VehicleDto> CreateAsync(CreateVehicleDto dto);
    }
}