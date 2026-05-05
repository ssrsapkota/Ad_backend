using Microsoft.AspNetCore.Mvc;
using Partpurja.Application.DTOs.Loyalty;
using Partpurja.Application.Interface.IServices;

namespace Partpurja.Api.Controllers
{
    /// <summary>
    /// API controller for loyalty program operations.
    /// Handles discount calculations for customer purchases.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class LoyaltyController : ControllerBase
    {
        private readonly ILoyaltyService _loyaltyService;

        public LoyaltyController(ILoyaltyService loyaltyService)
        {
            _loyaltyService = loyaltyService;
        }

        /// <summary>
        /// Calculates the loyalty discount for a given purchase amount.
        /// </summary>
        [HttpPost("calculate")]
        public async Task<ActionResult<LoyaltyCalculationResultDto>> Calculate(
            [FromBody] LoyaltyCalculationRequestDto request)
        {
            var result = await _loyaltyService.CalculateAsync(request);
            return Ok(result);
        }
    }
}
