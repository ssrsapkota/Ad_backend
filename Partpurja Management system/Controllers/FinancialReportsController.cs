using Microsoft.AspNetCore.Mvc;
using Partpurja.Application.DTOs.FinancialReport;
using Partpurja.Application.Interface.IServices;

namespace Partpurja.Api.Controllers
{
    
    
    
    [ApiController]
    [Route("api/admin/[controller]")]
    public class FinancialReportsController : ControllerBase
    {
        private readonly IFinancialReportService _reportService;

        public FinancialReportsController(IFinancialReportService reportService)
        {
            _reportService = reportService;
        }

        /// <summary>
        /// Returns a daily financial summary for the given date.
        /// Example: GET /api/admin/financialreports/daily?date=2026-05-18
        /// </summary>
        [HttpGet("daily")]
        public async Task<ActionResult<FinancialReportDto>> GetDailyReport([FromQuery] DateTime date)
        {
            if (date == default)
                return BadRequest(new { message = "Please provide a valid date." });

            var report = await _reportService.GetDailyReportAsync(date);
            return Ok(report);
        }

       
        [HttpGet("monthly")]
        public async Task<ActionResult<FinancialReportDto>> GetMonthlyReport(
            [FromQuery] int year, [FromQuery] int month)
        {
            if (year < 2000 || month < 1 || month > 12)
                return BadRequest(new { message = "Provide a valid year (≥ 2000) and month (1–12)." });

            var report = await _reportService.GetMonthlyReportAsync(year, month);
            return Ok(report);
        }

      
        [HttpGet("yearly")]
        public async Task<ActionResult<FinancialReportDto>> GetYearlyReport([FromQuery] int year)
        {
            if (year < 2000)
                return BadRequest(new { message = "Provide a valid year (≥ 2000)." });

            var report = await _reportService.GetYearlyReportAsync(year);
            return Ok(report);
        }
    }
}
