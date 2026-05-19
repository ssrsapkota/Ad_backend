using Partpurja.Application.DTOs.Report;
using Partpurja.Domain.Models;

namespace Partpurja.Application.Interface.IRepository
{
    public interface IReportRepository
    {
        Task<List<SalesInvoice>> GetSalesInvoicesInRangeAsync(DateTime startInclusive, DateTime endExclusive);
        Task<List<Part>> GetAllPartsAsync();
        Task<List<RegularCustomerDto>> GetRegularCustomersAsync(int minPurchaseCount);
        Task<List<HighSpenderDto>> GetHighSpendersAsync(int topN);
        Task<List<PendingCreditDto>> GetPendingCreditsAsync();
    }
}
