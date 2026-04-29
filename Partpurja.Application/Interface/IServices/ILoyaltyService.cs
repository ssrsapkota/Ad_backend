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
        /// Applies a 10% discount if the subtotal exceeds 5000.
        /// </summary>
        /// <param name="request">The loyalty calculation request containing the subtotal amount.</param>
        /// <returns>The loyalty calculation result with discount details.</returns>
        LoyaltyCalculationResultDto Calculate(LoyaltyCalculationRequestDto request);
    }
}
