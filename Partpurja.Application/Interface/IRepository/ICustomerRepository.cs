using Partpurja.Domain.Models.Users;

namespace Partpurja.Application.Interface.IRepository;

public interface ICustomerRepository
{
    Task AddAsync(Customer customer, CancellationToken ct = default);
    Task UpdateAsync(Customer customer, CancellationToken ct = default);
    Task<Customer?> GetByPhoneNumberAsync(string phoneNumber, CancellationToken ct = default);
}