namespace Partpurja.Application.DTOs.Loyalty
{
    /// <summary>
    /// Data transfer object for loyalty program discount calculation result.
    /// </summary>
    public class LoyaltyCalculationResultDto
    {
        /// <summary>
        /// The original subtotal amount before discount.
        /// </summary>
        public decimal SubTotal { get; set; }

        /// <summary>
        /// The calculated discount amount applied to the purchase.
        /// </summary>
        public decimal DiscountAmount { get; set; }

        /// <summary>
        /// The discount percentage applied (10 for 10%, 0 if no discount).
        /// </summary>
        public decimal DiscountPercentage { get; set; }

        /// <summary>
        /// The final total amount after discount.
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Indicates whether the loyalty discount was applied to this purchase.
        /// </summary>
        public bool IsLoyaltyDiscountApplied { get; set; }

        /// <summary>
        /// A descriptive message about the loyalty discount calculation.
        /// </summary>
        public string Message { get; set; } = string.Empty;
    }
}
