using Partpurja.Domain.Models;

namespace Partpurja.Application.DTOs.CreditReminder
{
    /// <summary>
    /// Data transfer object for reading credit reminder information.
    /// </summary>
    public class CreditReminderDto
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public string CustomerName { get; set; } = string.Empty;

        public string CustomerEmail { get; set; } = string.Empty;

        public int? SalesInvoiceId { get; set; }

        public decimal Amount { get; set; }

        public DateTime DueDate { get; set; }

        public bool IsEmailSent { get; set; }

        public CreditReminderStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}