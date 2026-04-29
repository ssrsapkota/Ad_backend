using Partpurja.Application.DTOs.Loyalty;
using Partpurja.Application.Interface.IServices;

namespace Partpurja.Infrastructure.Services
{
    /// <summary>
    /// Service for calculating loyalty program discounts.
    /// Provides a 10% discount for purchases exceeding Rs. 5000.
    /// </summary>
    public class LoyaltyService : ILoyaltyService
    {
        /// <summary>
        /// The minimum purchase amount required to qualify for a loyalty discount.
        /// </summary>
        private const decimal ThresholdAmount = 5000m;

        /// <summary>
        /// The discount rate applied to qualifying purchases (10%).
        /// </summary>
        private const decimal DiscountRate = 0.10m;

        /// <summary>
        /// Calculates the loyalty discount for a given purchase subtotal.
        /// </summary>
        /// <param name="request">The loyalty calculation request containing the subtotal amount.</param>
        /// <returns>The loyalty calculation result with discount details and total amount.</returns>
        public LoyaltyCalculationResultDto Calculate(LoyaltyCalculationRequestDto request)
        {
            decimal discountAmount = 0m;
            decimal discountPercentage = 0m;
            bool isLoyaltyDiscountApplied = false;
            string message = string.Empty;

            if (request.SubTotal > ThresholdAmount)
            {
                discountAmount = Math.Round(request.SubTotal * DiscountRate, 2, MidpointRounding.AwayFromZero);
                discountPercentage = 10m;
                isLoyaltyDiscountApplied = true;
                message = "10% loyalty discount applied (purchase exceeds Rs. 5000).";
            }
            else
            {
                discountAmount = 0m;
                discountPercentage = 0m;
                isLoyaltyDiscountApplied = false;
                message = "Spend more than Rs. 5000 in a single purchase to qualify for a 10% loyalty discount.";
            }

            decimal totalAmount = Math.Round(request.SubTotal - discountAmount, 2, MidpointRounding.AwayFromZero);

            return new LoyaltyCalculationResultDto
            {
                SubTotal = request.SubTotal,
                DiscountAmount = discountAmount,
                DiscountPercentage = discountPercentage,
                TotalAmount = totalAmount,
                IsLoyaltyDiscountApplied = isLoyaltyDiscountApplied,
                Message = message
            };
        }
    }
}
