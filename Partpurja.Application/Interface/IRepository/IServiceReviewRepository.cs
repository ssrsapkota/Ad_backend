using Partpurja.Domain.Models;

namespace Partpurja.Application.Interface.IRepository
{
    public interface IServiceReviewRepository
    {
        Task<List<ServiceReview>> GetAllAsync();

        Task<List<ServiceReview>> GetByCustomerIdAsync(int customerId);

        Task<ServiceReview> CreateAsync(ServiceReview review);
    }
}