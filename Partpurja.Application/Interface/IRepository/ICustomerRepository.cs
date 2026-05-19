using Partpurja.Domain.Models;

namespace Partpurja.Application.Interface.IRepository
{
    public interface ICustomerRepository
    {
        Task<List<Customer>> GetAllAsync();
        Task<Customer?> GetByIdAsync(int id);
        Task<Customer?> GetByUserIdAsync(int userId);
        Task<Customer> CreateAsync(Customer customer);
        Task<Customer?> UpdateAsync(int id, Customer customer);

        /// <summary>
        /// Searches customers by id, phone, name or vehicle registration number.
        /// Includes related User and Vehicles for richer results.
        /// </summary>
        Task<List<Customer>> SearchAsync(string query);
    }
}
