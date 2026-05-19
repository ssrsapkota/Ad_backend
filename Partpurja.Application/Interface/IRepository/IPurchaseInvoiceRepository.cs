using Partpurja.Domain.Models;

namespace Partpurja.Application.Interface.IRepository
{
    public interface IPurchaseInvoiceRepository
    {
        Task<List<PurchaseInvoice>> GetAllAsync();
        Task<PurchaseInvoice?> GetByIdAsync(int id);
        Task<PurchaseInvoice> CreateAsync(PurchaseInvoice invoice);
    }
}
