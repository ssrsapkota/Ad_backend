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

        /// <summary>
        /// Initializes a new instance of the LoyaltyController class.
        /// </summary>
        /// <param name="loyaltyService">The loyalty service for discount calculations.</param>
        public LoyaltyController(ILoyaltyService loyaltyService)
        {
            _loyaltyService = loyaltyService;
        }

        /// <summary>
        /// Calculates the loyalty discount for a given purchase amount.
        /// </summary>
        /// <param name="request">The loyalty calculation request containing the purchase subtotal.</param>
        /// <returns>The loyalty calculation result with discount details and total amount.</returns>
        [HttpPost("calculate")]
        public ActionResult<LoyaltyCalculationResultDto> Calculate([FromBody] LoyaltyCalculationRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (request.SubTotal < 0)
            {
                return BadRequest(new { message = "Subtotal cannot be negative." });
            }

            var result = _loyaltyService.Calculate(request);
            return Ok(result);
        }
    }
}
