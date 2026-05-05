using System.ComponentModel.DataAnnotations;

namespace Partpurja.Application.DTOs.Loyalty
{
    /// <summary>
    /// Data transfer object for loyalty program discount calculation request.
    /// </summary>
    public class LoyaltyCalculationRequestDto
    {
        /// <summary>
        /// The subtotal amount of the purchase in currency units.
        /// </summary>
        [Range(0, double.MaxValue, ErrorMessage = "SubTotal cannot be negative.")]
        public decimal SubTotal { get; set; }
    }
}
