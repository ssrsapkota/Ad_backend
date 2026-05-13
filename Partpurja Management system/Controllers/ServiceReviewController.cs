using Microsoft.AspNetCore.Mvc;
using Partpurja.Application.DTOs.ServiceReview;
using Partpurja.Application.Interface.IServices;

namespace Partpurja_Management_system.Controllers
{
    [ApiController]
    [Route("api/service-reviews")]
    public class ServiceReviewsController : ControllerBase
    {
        private readonly IServiceReviewService _service;

        public ServiceReviewsController(IServiceReviewService service)
        {
            _service = service;
        }

        // Get all reviews
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var reviews = await _service.GetAllAsync();
            return Ok(reviews);
        }

        // Get reviews by customer
        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetByCustomerId(int customerId)
        {
            var reviews = await _service.GetByCustomerIdAsync(customerId);
            return Ok(reviews);
        }

        // Create review
        [HttpPost]
        public async Task<IActionResult> Create(CreateServiceReviewDto dto)
        {
            var review = await _service.CreateAsync(dto);
            return Ok(review);
        }
    }
}