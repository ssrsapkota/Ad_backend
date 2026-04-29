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
        public decimal SubTotal { get; set; }
    }
}
