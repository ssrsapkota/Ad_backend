using Partpurja.Application.DTOs.Customers;

namespace Partpurja.Application.Interface.IServices;

public interface ICustomerService
{
    Task<CustomerDto> RegisterCustomerWithVehicleAsync(RegisterCustomerRequestDto dto, CancellationToken ct = default);
}