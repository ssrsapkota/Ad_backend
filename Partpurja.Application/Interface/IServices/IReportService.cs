using Partpurja.Application.DTOs.Report;

namespace Partpurja.Application.Interface.IServices
{
    public interface IReportService
    {
        Task<FinancialReportDto> GetDailyReportAsync(DateTime day);
        Task<FinancialReportDto> GetMonthlyReportAsync(int year, int month);
        Task<FinancialReportDto> GetYearlyReportAsync(int year);
        Task<List<InventoryReportItemDto>> GetInventoryReportAsync();
        Task<List<RegularCustomerDto>> GetRegularCustomersAsync(int minPurchaseCount = 3);
        Task<List<HighSpenderDto>> GetHighSpendersAsync(int topN = 10);
        Task<List<PendingCreditDto>> GetPendingCreditsAsync();
    }
}
