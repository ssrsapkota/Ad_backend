using Partpurja.Application.DTOs.Customer;
using Partpurja.Application.Interface.IRepository;
using Partpurja.Application.Interface.IServices;
using Partpurja.Domain.Models;

namespace Partpurja.Infrastructure.Services
{
    // Service implementation for Customer
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repo;

        public CustomerService(ICustomerRepository repo)
        {
            // Dependency injection
            _repo = repo;
        }

        // Method to get all customers
        public async Task<List<CustomerDto>> GetAllAsync()
            {
                //Repository method to get all customers
                var customers = await _repo.GetAllAsync();

                return customers.Select(c => new CustomerDto
                {
                    Id = c.Id,
                    FullName = c.FullName,
                    Phone = c.Phone,
                    Address = c.Address
                }).ToList();
            }

        // Method to create a new customer
        public async Task<CustomerDto> CreateAsync(CreateCustomerDto dto)
        {
            // Map the CreateCustomerDto to Customer entity
            var customer = new Customer
            {
                FullName = dto.FullName,
                Phone = dto.Phone,
                Address = dto.Address
            };

            //Repository method for New Customer
            var created = await _repo.CreateAsync(customer);

            return new CustomerDto
            {
                Id = created.Id,
                FullName = created.FullName,
                Phone = created.Phone,
                Address = created.Address
            };
        }
    }
}
