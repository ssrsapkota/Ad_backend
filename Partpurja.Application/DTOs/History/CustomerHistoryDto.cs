namespace Partpurja.Application.DTOs.History
{
public class CustomerHistoryDto
{
    //Purchase and service history of a customer
    public List<PurchaseHistoryDto> Purchases { get; set; } = new();
    public List<ServiceHistoryDto> Services { get; set; } = new();
}
}