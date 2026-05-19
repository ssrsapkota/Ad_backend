using Partpurja.Application.DTOs.Customer;
using Partpurja.Application.Interface.IRepository;
using Partpurja.Application.Interface.IServices;
using Partpurja.Domain.Models;

namespace Partpurja.Infrastructure.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repo;

        public CustomerService(ICustomerRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<CustomerDto>> GetAllAsync()
        {
            var customers = await _repo.GetAllAsync();
            return customers.Select(Map).ToList();
        }

        public async Task<CustomerDto?> GetByIdAsync(int id)
        {
            var customer = await _repo.GetByIdAsync(id);
            return customer == null ? null : Map(customer);
        }

        public async Task<CustomerDto?> GetByUserIdAsync(int userId)
        {
            var customer = await _repo.GetByUserIdAsync(userId);
            return customer == null ? null : Map(customer);
        }

        public async Task<CustomerDto> CreateAsync(CreateCustomerDto dto)
        {
            var customer = new Customer
            {
                UserId = dto.UserId,
                FullName = dto.FullName,
                Phone = dto.Phone,
                Address = dto.Address
            };

            if (!string.IsNullOrWhiteSpace(dto.VehicleRegistrationNumber))
            {
                customer.Vehicles.Add(new Vehicle
                {
                    RegistrationNumber = dto.VehicleRegistrationNumber,
                    Brand = dto.VehicleBrand ?? string.Empty,
                    Model = dto.VehicleModel ?? string.Empty,
                    Year = dto.VehicleYear ?? DateTime.UtcNow.Year,
                    ChassisNumber = dto.VehicleChassisNumber ?? string.Empty,
                    VehicleCondition = dto.VehicleCondition ?? string.Empty,
                    MonthlyUsageKm = dto.MonthlyUsageKm ?? 0,
                    IsActive = true
                });
            }

            var created = await _repo.CreateAsync(customer);
            return Map(created);
        }

        public async Task<CustomerDto?> UpdateAsync(int id, UpdateCustomerDto dto)
        {
            var updated = await _repo.UpdateAsync(id, new Customer
            {
                FullName = dto.FullName,
                Phone = dto.Phone,
                Address = dto.Address
            });

            return updated == null ? null : Map(updated);
        }

        public async Task<List<CustomerSearchResultDto>> SearchAsync(string query)
        {
            var customers = await _repo.SearchAsync(query);

            return customers.Select(c => new CustomerSearchResultDto
            {
                Id = c.Id,
                FullName = c.FullName,
                Phone = c.Phone,
                Email = c.User?.Email ?? string.Empty,
                Address = c.Address,
                VehicleRegistrationNumbers = c.Vehicles
                    .Where(v => v.IsActive)
                    .Select(v => v.RegistrationNumber)
                    .ToList()
            }).ToList();
        }

        private static CustomerDto Map(Customer c) => new()
        {
            Id = c.Id,
            FullName = c.FullName,
            Phone = c.Phone,
            Address = c.Address
        };
    }
}
