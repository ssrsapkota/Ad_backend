using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Partpurja.Application.Interface.IServices;

namespace Partpurja_Management_system.Controllers
{
    // Controller for handling customer history related requests
    [ApiController]
    [Route("api/history")]
    [Authorize(Roles = "Admin,Staff,Customer")]
    public class HistoryController : ControllerBase
    {
        private readonly IHistoryService _service;

        public HistoryController(IHistoryService service)
        {
            _service = service;
        }
        //Get API to retrieve the purchase and service history 
        [HttpGet("{customerId}")]
        public async Task<IActionResult> GetHistory(int customerId)
        {
            var result = await _service.GetCustomerHistoryAsync(customerId);
            return Ok(result);
        }
    }
}