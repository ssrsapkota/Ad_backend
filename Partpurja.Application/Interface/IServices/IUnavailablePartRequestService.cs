using Partpurja.Application.DTOs.UnavailablePartRequest;

namespace Partpurja.Application.Interface.IServices
{
    public interface IUnavailablePartRequestService
    {
        Task<List<UnavailablePartRequestDto>> GetAllAsync();

        Task<List<UnavailablePartRequestDto>> GetByCustomerIdAsync(int customerId);

        Task<UnavailablePartRequestDto> CreateAsync(CreateUnavailablePartRequestDto dto);
    }
}