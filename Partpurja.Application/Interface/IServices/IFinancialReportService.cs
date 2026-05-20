using Partpurja.Application.DTOs.FinancialReport;

namespace Partpurja.Application.Interface.IServices
{
   
    public interface IFinancialReportService
    {
        
        Task<FinancialReportDto> GetDailyReportAsync(DateTime date);

       
        Task<FinancialReportDto> GetMonthlyReportAsync(int year, int month);

        
        Task<FinancialReportDto> GetYearlyReportAsync(int year);
    }
}