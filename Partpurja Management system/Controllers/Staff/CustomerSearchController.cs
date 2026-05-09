using Microsoft.AspNetCore.Mvc;
using Partpurja.Application.DTOs.Search;
using Partpurja.Application.Interface.IServices;

namespace Partpurja_Management_system.Controllers.Staff;

[ApiController]
[Route("api/staff/customers/search")]
public class CustomerSearchController : ControllerBase
{
    private readonly ISearchService _searchService;

    public CustomerSearchController(ISearchService searchService)
    {
        _searchService = searchService;
    }

    [HttpGet]
    public async Task<ActionResult<CustomerSearchResultDto>> Search([FromQuery] string? searchTerm, CancellationToken ct)
    {
        try
        {
            var request = new CustomerSearchRequestDto { SearchTerm = searchTerm };
            var result = await _searchService.SearchCustomersAsync(request, ct);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}
