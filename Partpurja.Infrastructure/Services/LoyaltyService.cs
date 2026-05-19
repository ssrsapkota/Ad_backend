using Partpurja.Application.DTOs.Loyalty;
using Partpurja.Application.Interface.IServices;

namespace Partpurja.Infrastructure.Services
{
    /// <summary>
    /// Service for calculating loyalty program discounts.
    /// Provides a 10% discount for purchases of Rs. 5000 or above.
    /// </summary>
    public class LoyaltyService : ILoyaltyService
    {
        private const decimal ThresholdAmount = 5000m;
        private const decimal DiscountRate = 0.10m;

        public Task<LoyaltyCalculationResultDto> CalculateAsync(LoyaltyCalculationRequestDto request)
        {
            bool isLoyaltyDiscountApplied = request.SubTotal > ThresholdAmount;

            decimal discountAmount = isLoyaltyDiscountApplied
                ? Math.Round(request.SubTotal * DiscountRate, 2, MidpointRounding.AwayFromZero)
                : 0m;

            decimal discountPercentage = isLoyaltyDiscountApplied ? DiscountRate * 100m : 0m;

            decimal totalAmount = Math.Round(request.SubTotal - discountAmount, 2, MidpointRounding.AwayFromZero);

            string message = isLoyaltyDiscountApplied
                ? $"{discountPercentage:0}% loyalty discount applied (purchase strictly exceeds Rs. 5000)."
                : "Spend more than Rs. 5000 in a single purchase to qualify for a 10% loyalty discount.";

            return Task.FromResult(new LoyaltyCalculationResultDto
            {
                SubTotal = request.SubTotal,
                DiscountAmount = discountAmount,
                DiscountPercentage = discountPercentage,
                TotalAmount = totalAmount,
                IsLoyaltyDiscountApplied = isLoyaltyDiscountApplied,
                Message = message
            });
        }
    }
}
