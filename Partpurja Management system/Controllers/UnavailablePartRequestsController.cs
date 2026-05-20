using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Partpurja.Application.DTOs.UnavailablePartRequest;
using Partpurja.Application.Interface.IServices;

namespace Partpurja_Management_system.Controllers
{
    [ApiController]
    [Route("api/unavailable-part-requests")]
    [Authorize]
    public class UnavailablePartRequestsController : ControllerBase
    {
        private readonly IUnavailablePartRequestService _service;

        public UnavailablePartRequestsController(IUnavailablePartRequestService service)
        {
            _service = service;
        }

        // Get all requests
        [HttpGet]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> GetAll()
        {
            var requests = await _service.GetAllAsync();
            return Ok(requests);
        }

        // Get requests by customer
        [HttpGet("customer/{customerId}")]
        [Authorize(Roles = "Admin,Staff,Customer")]
        public async Task<IActionResult> GetByCustomerId(int customerId)
        {
            var requests = await _service.GetByCustomerIdAsync(customerId);
            return Ok(requests);
        }

        // Create request
        [HttpPost]
        [Authorize(Roles = "Admin,Staff,Customer")]
        public async Task<IActionResult> Create(CreateUnavailablePartRequestDto dto)
        {
            var request = await _service.CreateAsync(dto);
            return Ok(request);
        }
    }
}