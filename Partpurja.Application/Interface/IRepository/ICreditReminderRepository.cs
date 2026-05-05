using Partpurja.Application.DTOs.CreditReminder;
using Partpurja.Domain.Models;

namespace Partpurja.Application.Interface.IRepository
{
    /// <summary>
    /// Interface for credit reminder persistence operations.
    /// </summary>
    public interface ICreditReminderRepository
    {
        /// <summary>
        /// Gets all credit reminders ordered by due date.
        /// </summary>
        Task<IEnumerable<CreditReminderDto>> GetAllAsync();

        /// <summary>
        /// Gets all sales invoices with outstanding credit older than the given days.
        /// Used to find which customers to email.
        /// </summary>
        Task<IEnumerable<SalesInvoice>> GetOverdueInvoicesAsync(int overdueDays);

        /// <summary>
        /// Checks if a reminder already exists for a sales invoice (deduplication).
        /// </summary>
        Task<bool> ExistsForInvoiceAsync(int salesInvoiceId);

        /// <summary>
        /// Creates a new credit reminder linked to a sales invoice.
        /// </summary>
        Task<CreditReminderDto> CreateAsync(int customerId, int salesInvoiceId, decimal amount, DateTime dueDate);

        /// <summary>
        /// Marks the email-sent flag on a reminder after it's been emailed.
        /// </summary>
        Task<bool> MarkEmailSentAsync(int id);
    }
}