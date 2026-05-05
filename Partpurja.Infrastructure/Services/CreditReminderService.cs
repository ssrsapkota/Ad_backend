using Microsoft.Extensions.Logging;
using Partpurja.Application.DTOs.CreditReminder;
using Partpurja.Application.DTOs.Email;
using Partpurja.Application.Interface.IRepository;
using Partpurja.Application.Interface.IServices;

namespace Partpurja.Infrastructure.Services
{
    public class CreditReminderService : ICreditReminderService
    {
        private const int OverdueDays = 30;

        private readonly ICreditReminderRepository _creditReminderRepository;
        private readonly IEmailService _emailService;
        private readonly ILogger<CreditReminderService> _logger;

        public CreditReminderService(
            ICreditReminderRepository creditReminderRepository,
            IEmailService emailService,
            ILogger<CreditReminderService> logger)
        {
            _creditReminderRepository = creditReminderRepository;
            _emailService = emailService;
            _logger = logger;
        }

        public async Task<IEnumerable<CreditReminderDto>> GetAllAsync()
        {
            return await _creditReminderRepository.GetAllAsync();
        }

        public async Task<int> ProcessOverdueCreditsAsync()
        {
            var overdueInvoices = (await _creditReminderRepository.GetOverdueInvoicesAsync(OverdueDays)).ToList();

            if (overdueInvoices.Count == 0)
            {
                _logger.LogInformation("No overdue credit invoices found.");
                return 0;
            }

            var emailsSent = 0;

            foreach (var invoice in overdueInvoices)
            {
                var alreadyReminded = await _creditReminderRepository.ExistsForInvoiceAsync(invoice.Id);
                if (alreadyReminded)
                    continue;

                var dueDate = invoice.Date.AddDays(OverdueDays);
                var reminder = await _creditReminderRepository.CreateAsync(
                    invoice.CustomerId, invoice.Id, invoice.CreditAmount, dueDate);

                var customerEmail = invoice.Customer?.User?.Email ?? string.Empty;
                var customerName = invoice.Customer?.FullName ?? "Customer";

                if (string.IsNullOrEmpty(customerEmail))
                {
                    _logger.LogWarning(
                        "No email address for customer {CustomerId}, skipping reminder for invoice {InvoiceId}.",
                        invoice.CustomerId, invoice.Id);
                    continue;
                }

                var sent = await _emailService.SendEmailAsync(new EmailMessageDto
                {
                    To = customerEmail,
                    ToName = customerName,
                    Subject = $"Payment Reminder — Invoice #{invoice.InvoiceNumber}",
                    Body = BuildEmailBody(customerName, invoice.InvoiceNumber, reminder.Amount, reminder.DueDate)
                });

                if (sent)
                {
                    await _creditReminderRepository.MarkEmailSentAsync(reminder.Id);
                    emailsSent++;
                }
                else
                {
                    _logger.LogWarning("Failed to send credit reminder email for invoice {InvoiceId}.", invoice.Id);
                }
            }

            _logger.LogInformation("Credit reminder processing complete. {Count} email(s) sent.", emailsSent);
            return emailsSent;
        }

        private static string BuildEmailBody(string customerName, string invoiceNumber, decimal amount, DateTime dueDate)
        {
            return $"""
                <p>Dear {customerName},</p>
                <p>This is a friendly reminder that your credit payment of
                <strong>Rs. {amount:N2}</strong> for invoice <strong>#{invoiceNumber}</strong>
                was due on <strong>{dueDate:MMMM d, yyyy}</strong>.</p>
                <p>Please contact us to arrange payment at your earliest convenience.</p>
                <p>Thank you,<br/>Partpurja Management System</p>
                """;
        }
    }
}
