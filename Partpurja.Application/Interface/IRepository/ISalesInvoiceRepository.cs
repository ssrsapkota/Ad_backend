using Partpurja.Domain.Models;

namespace Partpurja.Application.Interface.IRepository
{
    public interface ISalesInvoiceRepository
    {
        Task<List<SalesInvoice>> GetByCustomerIdAsync(int customerId);
        Task<SalesInvoice?> GetByIdAsync(int id);
        Task<SalesInvoice?> GetByIdWithCustomerAsync(int id);
        Task<SalesInvoice> CreateAsync(SalesInvoice invoice);
        Task<bool> MarkEmailedAsync(int id);
    }
}
