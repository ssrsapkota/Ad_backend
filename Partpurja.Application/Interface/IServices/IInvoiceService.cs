using Partpurja.Application.DTOs.Invoice;

namespace Partpurja.Application.Interface.IServices
{
    public interface IInvoiceService
    {
        Task<InvoiceDto?> GetByIdAsync(int id);
        Task<List<InvoiceDto>> GetByCustomerIdAsync(int customerId);
        Task<InvoiceDto> CreateAsync(CreateInvoiceDto dto);

        /// <summary>
        /// Emails the sales invoice to the customer on file and flips IsInvoiceEmailed.
        /// Returns true if the email was successfully sent.
        /// </summary>
        Task<bool> SendInvoiceEmailAsync(int invoiceId);
    }
}
