using Partpurja.Application.DTOs.Customer;

namespace Partpurja.Application.Interface.IServices
{
    public interface ICustomerService
    {
        // Method to get all customers
        Task<List<CustomerDto>> GetAllAsync();
        // Method to create a new customer
        Task<CustomerDto> CreateAsync(CreateCustomerDto dto);
    }
}