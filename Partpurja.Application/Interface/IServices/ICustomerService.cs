using Partpurja.Application.DTOs.Customer;

namespace Partpurja.Application.Interface.IServices
{
    public interface ICustomerService
    {
        Task<List<CustomerDto>> GetAllAsync();
        Task<CustomerDto?> GetByIdAsync(int id);
        Task<CustomerDto?> GetByUserIdAsync(int userId);
        Task<CustomerDto> CreateAsync(CreateCustomerDto dto);
        Task<CustomerDto?> UpdateAsync(int id, UpdateCustomerDto dto);

        /// <summary>
        /// Searches customers by id, phone, name or vehicle registration number.
        /// </summary>
        Task<List<CustomerSearchResultDto>> SearchAsync(string query);
    }
}
