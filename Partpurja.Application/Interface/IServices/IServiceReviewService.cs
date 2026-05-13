using Partpurja.Application.DTOs.ServiceReview;

namespace Partpurja.Application.Interface.IServices
{
    public interface IServiceReviewService
    {
        Task<List<ServiceReviewDto>> GetAllAsync();

        Task<List<ServiceReviewDto>> GetByCustomerIdAsync(int customerId);

        Task<ServiceReviewDto> CreateAsync(CreateServiceReviewDto dto);
    }
}