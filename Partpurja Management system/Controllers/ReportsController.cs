using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Partpurja.Application.Interface.IServices;

namespace Partpurja_Management_system.Controllers
{
    [ApiController]
    [Route("api/reports")]
    [Authorize]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _service;

        public ReportsController(IReportService service)
        {
            _service = service;
        }

        [HttpGet("daily")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Daily([FromQuery] DateTime? date)
        {
            var day = date ?? DateTime.UtcNow.Date;
            return Ok(await _service.GetDailyReportAsync(day));
        }

        [HttpGet("monthly")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Monthly([FromQuery] int year, [FromQuery] int month)
        {
            if (year < 1 || month < 1 || month > 12)
            {
                return BadRequest(new { message = "Provide a valid year and month (1–12)." });
            }

            try
            {
                return Ok(await _service.GetMonthlyReportAsync(year, month));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("yearly")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Yearly([FromQuery] int year)
        {
            if (year < 1) return BadRequest(new { message = "Provide a valid year." });
            return Ok(await _service.GetYearlyReportAsync(year));
        }

        [HttpGet("inventory")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Inventory() => Ok(await _service.GetInventoryReportAsync());

        [HttpGet("regular-customers")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> RegularCustomers([FromQuery] int minPurchaseCount = 3)
        {
            try
            {
                return Ok(await _service.GetRegularCustomersAsync(minPurchaseCount));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("high-spenders")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> HighSpenders([FromQuery] int topN = 10)
        {
            try
            {
                return Ok(await _service.GetHighSpendersAsync(topN));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("pending-credits")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> PendingCredits() => Ok(await _service.GetPendingCreditsAsync());
    }
}
