using Partpurja.Domain.Models;

namespace Partpurja.Application.Interface.IRepository
{
    public interface IUnavailablePartRequestRepository
    {
        Task<List<UnavailablePartRequest>> GetAllAsync();

        Task<List<UnavailablePartRequest>> GetByCustomerIdAsync(int customerId);

        Task<UnavailablePartRequest> CreateAsync(UnavailablePartRequest request);
    }
}