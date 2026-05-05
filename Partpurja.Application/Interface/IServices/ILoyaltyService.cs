using Partpurja.Application.DTOs.Loyalty;

namespace Partpurja.Application.Interface.IServices
{
    /// <summary>
    /// Interface for loyalty program discount calculation service.
    /// </summary>
    public interface ILoyaltyService
    {
        /// <summary>
        /// Calculates the loyalty discount for a given purchase subtotal.
        /// Applies a 10% discount if the subtotal is 5000 or above.
        /// </summary>
        Task<LoyaltyCalculationResultDto> CalculateAsync(LoyaltyCalculationRequestDto request);
    }
}
