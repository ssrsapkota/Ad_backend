using Partpurja.Application.DTOs.PurchaseInvoice;

namespace Partpurja.Application.Interface.IServices
{
    public interface IPurchaseInvoiceService
    {
        Task<List<PurchaseInvoiceDto>> GetAllAsync();
        Task<PurchaseInvoiceDto?> GetByIdAsync(int id);
        Task<PurchaseInvoiceDto> CreateAsync(CreatePurchaseInvoiceDto dto);
    }
}
