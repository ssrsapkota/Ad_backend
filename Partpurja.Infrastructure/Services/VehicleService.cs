using Partpurja.Application.DTOs.Vehicle;
using Partpurja.Application.Interface.IRepository;
using Partpurja.Application.Interface.IServices;
using Partpurja.Domain.Models;

namespace Partpurja.Infrastructure.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly IVehicleRepository _repo;

        public VehicleService(IVehicleRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<VehicleDto>> GetAllAsync()
        {
            var vehicles = await _repo.GetAllAsync();
            return vehicles.Select(Map).ToList();
        }

        public async Task<VehicleDto?> GetByIdAsync(int id)
        {
            var v = await _repo.GetByIdAsync(id);
            return v == null ? null : Map(v);
        }

        public async Task<List<VehicleDto>> GetByCustomerIdAsync(int customerId)
        {
            var vehicles = await _repo.GetByCustomerIdAsync(customerId);
            return vehicles.Select(Map).ToList();
        }

        public async Task<VehicleDto> CreateAsync(CreateVehicleDto dto)
        {
            var vehicle = new Vehicle
            {
                CustomerId = dto.CustomerId,
                RegistrationNumber = dto.RegistrationNumber,
                Brand = dto.Brand,
                Model = dto.Model,
                Year = dto.Year,
                ChassisNumber = dto.ChassisNumber,
                VehicleCondition = dto.VehicleCondition,
                MonthlyUsageKm = dto.MonthlyUsageKm
            };

            var created = await _repo.CreateAsync(vehicle);
            return Map(created);
        }

        public async Task<VehicleDto?> UpdateAsync(int id, UpdateVehicleDto dto)
        {
            var updated = await _repo.UpdateAsync(id, new Vehicle
            {
                RegistrationNumber = dto.RegistrationNumber,
                Brand = dto.Brand,
                Model = dto.Model,
                Year = dto.Year,
                ChassisNumber = dto.ChassisNumber,
                VehicleCondition = dto.VehicleCondition,
                MonthlyUsageKm = dto.MonthlyUsageKm
            });

            return updated == null ? null : Map(updated);
        }

        public Task<bool> DeleteAsync(int id) => _repo.DeleteAsync(id);

        private static VehicleDto Map(Vehicle v) => new()
        {
            Id = v.Id,
            CustomerId = v.CustomerId,
            RegistrationNumber = v.RegistrationNumber,
            Brand = v.Brand,
            Model = v.Model,
            Year = v.Year,
            ChassisNumber = v.ChassisNumber,
            VehicleCondition = v.VehicleCondition,
            MonthlyUsageKm = v.MonthlyUsageKm
        };
    }
}
