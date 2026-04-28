using Microsoft.AspNetCore.Mvc;
using Partpurja.Application.DTOs.Customer;
using Partpurja.Application.Interface.IServices;

namespace Partpurja_Management_system.Controllers
{
    // Controller for Customer Management
    [ApiController]
    [Route("api/customers")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _service;

        public CustomersController(ICustomerService service)
        {
            // Dependency injection
            _service = service;
        }

        //Get API to get all customers
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var customers = await _service.GetAllAsync();
            return Ok(customers);
        }

        //Post API to create a new customer
        [HttpPost]
        public async Task<IActionResult> Create(CreateCustomerDto dto)
        {
            var customer = await _service.CreateAsync(dto);
            return Ok(customer);
        }
    }
}