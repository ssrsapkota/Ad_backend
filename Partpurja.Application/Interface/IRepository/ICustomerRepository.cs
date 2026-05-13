using Partpurja.Domain.Models;

namespace Partpurja.Application.Interface.IRepository
{
    public interface ICustomerRepository
    {
        // Method to get all customers
        Task<List<Customer>> GetAllAsync();
        // Method to get a customer by ID
        Task<Customer?> GetByIdAsync(int id);
        // Method to create a new customer
        Task<Customer> CreateAsync(Customer customer);
        // Method to update an existing customer
        Task<Customer?> UpdateAsync(int id, Customer customer);
    }
}