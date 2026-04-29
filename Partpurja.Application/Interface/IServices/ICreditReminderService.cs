using Partpurja.Application.DTOs.CreditReminder;

namespace Partpurja.Application.Interface.IServices
{
    /// <summary>
    /// Service for credit reminder business logic.
    /// </summary>
    public interface ICreditReminderService
    {
        /// <summary>
        /// Gets all credit reminders.
        /// </summary>
        Task<IEnumerable<CreditReminderDto>> GetAllAsync();

        /// <summary>
        /// Scans for sales invoices with credit unpaid for more than 30 days,
        /// creates reminder records, and emails the customers.
        /// Returns the number of emails successfully sent.
        /// </summary>
        Task<int> ProcessOverdueCreditsAsync();
    }
}