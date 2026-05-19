using Microsoft.EntityFrameworkCore;
using Partpurja.Application.Interface.IRepository;
using Partpurja.Domain.Models;
using Partpurja.Infrastructure.Persistence;

namespace Partpurja.Infrastructure.Repository
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly AppDbContext _context;

        public VehicleRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Vehicle>> GetAllAsync()
        {
            return await _context.Vehicles.ToListAsync();
        }

        public async Task<Vehicle?> GetByIdAsync(int id)
        {
            return await _context.Vehicles.FindAsync(id);
        }

        public async Task<List<Vehicle>> GetByCustomerIdAsync(int customerId)
        {
            return await _context.Vehicles
                .Where(v => v.CustomerId == customerId)
                .ToListAsync();
        }

        public async Task<Vehicle> CreateAsync(Vehicle vehicle)
        {
            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();
            return vehicle;
        }

        public async Task<Vehicle?> UpdateAsync(int id, Vehicle vehicle)
        {
            var existing = await _context.Vehicles.FindAsync(id);
            if (existing == null) return null;

            existing.RegistrationNumber = vehicle.RegistrationNumber;
            existing.Brand = vehicle.Brand;
            existing.Model = vehicle.Model;
            existing.Year = vehicle.Year;
            existing.ChassisNumber = vehicle.ChassisNumber;
            existing.VehicleCondition = vehicle.VehicleCondition;
            existing.MonthlyUsageKm = vehicle.MonthlyUsageKm;
            existing.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _context.Vehicles.FindAsync(id);
            if (existing == null) return false;

            existing.IsActive = false;
            existing.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
