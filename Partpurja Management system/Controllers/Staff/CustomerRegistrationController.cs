using Microsoft.AspNetCore.Mvc;
using Partpurja.Application.DTOs.Customers;
using Partpurja.Application.Interface.IServices;

namespace Partpurja_Management_system.Controllers.Staff;

[ApiController]
[Route("api/staff/customers")]
public class CustomerRegistrationController : ControllerBase
{
    private readonly ICustomerService _customerService;
    public CustomerRegistrationController(ICustomerService customerService) => _customerService = customerService;

    [HttpPost("register")]
    public async Task<ActionResult<CustomerDto>> Register([FromBody] RegisterCustomerRequestDto dto, CancellationToken ct)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var created = await _customerService.RegisterCustomerWithVehicleAsync(dto, ct);
            return Ok(created);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}