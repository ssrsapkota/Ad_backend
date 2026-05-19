using Microsoft.AspNetCore.Mvc;
using Partpurja.Application.DTOs.Customer;
using Partpurja.Application.Interface.IServices;

namespace Partpurja_Management_system.Controllers
{
    [ApiController]
    [Route("api/customers")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _service;

        public CustomersController(ICustomerService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var customers = await _service.GetAllAsync();
            return Ok(customers);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var customer = await _service.GetByIdAsync(id);
            if (customer == null) return NotFound(new { message = "Customer not found." });
            return Ok(customer);
        }

        [HttpGet("by-user/{userId:int}")]
        public async Task<IActionResult> GetByUserId(int userId)
        {
            var customer = await _service.GetByUserIdAsync(userId);
            if (customer == null) return NotFound(new { message = "Customer profile not found for this user." });
            return Ok(customer);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCustomerDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var customer = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = customer.Id }, customer);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCustomerDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var updated = await _service.UpdateAsync(id, dto);
            if (updated == null) return NotFound(new { message = "Customer not found." });
            return Ok(updated);
        }

        /// <summary>
        /// Staff-facing customer search by id, phone, name, or vehicle registration number.
        /// </summary>
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string q)
        {
            if (string.IsNullOrWhiteSpace(q))
            {
                return BadRequest(new { message = "Query parameter 'q' is required." });
            }

            var results = await _service.SearchAsync(q);
            return Ok(results);
        }
    }
}
