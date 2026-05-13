using Partpurja.Application.DTOs.History;

public interface IHistoryService
{
    // Method to get the purchase and service history of a customer
    Task<CustomerHistoryDto> GetCustomerHistoryAsync(int customerId);
}