using Partpurja.Application.DTOs.History;
using Partpurja.Application.Interface.IRepository;
using Partpurja.Application.Interface.IServices;

public class HistoryService : IHistoryService
{
    private readonly ISalesInvoiceRepository _salesRepo;
    private readonly IAppointmentRepository _appointmentRepo;

    // Constructor Injection
    public HistoryService(
        ISalesInvoiceRepository salesRepo,
        IAppointmentRepository appointmentRepo)
    {
        _salesRepo = salesRepo;
        _appointmentRepo = appointmentRepo;
    }
    // Method to get the purchase and service history of a customer
    public async Task<CustomerHistoryDto> GetCustomerHistoryAsync(int customerId)
    {
        var purchases = await _salesRepo.GetByCustomerIdAsync(customerId);
        var services = await _appointmentRepo.GetByCustomerIdAsync(customerId);

        return new CustomerHistoryDto
        {
            //Map to DTOs
            Purchases = purchases.Select(p => new PurchaseHistoryDto
            {
                Id = p.Id,
                Date = p.CreatedAt,
                TotalAmount = p.TotalAmount
            }).ToList(),

            Services = services.Select(s => new ServiceHistoryDto
            {
                Id = s.Id,
                Date = s.CreatedAt,
                Status = s.Status.ToString()
            }).ToList()
        };
    }
}