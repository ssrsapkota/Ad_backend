using Partpurja.Application.DTOs.Search;

namespace Partpurja.Application.Interface.IServices;

public interface ISearchService
{
    Task<CustomerSearchResultDto> SearchCustomersAsync(CustomerSearchRequestDto request, CancellationToken ct = default);
}