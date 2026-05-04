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
            //Repository method to get all vehicles
            var vehicles = await _repo.GetAllAsync();

            return vehicles.Select(v => new VehicleDto
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
            }).ToList();
        }

        public async Task<VehicleDto> CreateAsync(CreateVehicleDto dto)
        {
            // MapCreateVehicleDto to Vehicle entity
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
            //Repository method for New Vehicle
            var created = await _repo.CreateAsync(vehicle);

            return new VehicleDto
            {
                Id = created.Id,
                CustomerId = created.CustomerId,
                RegistrationNumber = created.RegistrationNumber,
                Brand = created.Brand,
                Model = created.Model,
                Year = created.Year,
                ChassisNumber = created.ChassisNumber,
                VehicleCondition = created.VehicleCondition,
                MonthlyUsageKm = created.MonthlyUsageKm
            };
        }
    }
}