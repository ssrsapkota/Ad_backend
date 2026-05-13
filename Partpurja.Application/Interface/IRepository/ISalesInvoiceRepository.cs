using Partpurja.Domain.Models;

public interface ISalesInvoiceRepository
{
    // Method to get all sales invoices
    Task<List<SalesInvoice>> GetByCustomerIdAsync(int customerId);
}