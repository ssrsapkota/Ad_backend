using Partpurja.Application.DTOs.Customers;
using Partpurja.Application.Interface.IRepository;
using Partpurja.Application.Interface.IServices;
using Partpurja.Domain.Models.Users;
using Partpurja.Domain.Models.Vehicle;

namespace Partpurja.Application.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customers;
    private readonly IVehicleRepository _vehicles;
    public CustomerService(ICustomerRepository customers, IVehicleRepository vehicles)
    {
        _customers = customers;
        _vehicles = vehicles;
    }

    public async Task<CustomerDto> RegisterCustomerWithVehicleAsync(RegisterCustomerRequestDto dto, CancellationToken ct = default)
    {
        // Check if customer already exists by phone number
        var customer = await _customers.GetByPhoneNumberAsync(dto.PhoneNumber, ct);
        
        if (customer == null)
        {
            // Create new customer
            customer = new Customer
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                PhoneNumber = dto.PhoneNumber,
                Email = dto.Email,
                Address = dto.Address
            };
            
            var vehicle = new VehicleInfo
            {
                VehicleNumber = dto.VehicleNumber,
                Brand = dto.Brand,
                Model = dto.Model,
                Year = dto.Year,
                Customer = customer
            };
 
            customer.Vehicles.Add(vehicle);
            await _customers.AddAsync(customer, ct);
        }
        else
        {
            // Check if vehicle already exists for this customer
            if (!customer.Vehicles.Any(v => v.VehicleNumber == dto.VehicleNumber))
            {
                var vehicle = new VehicleInfo
                {
                    VehicleNumber = dto.VehicleNumber,
                    Brand = dto.Brand,
                    Model = dto.Model,
                    Year = dto.Year,
                    Customer = customer
                };
                customer.Vehicles.Add(vehicle);
                await _customers.UpdateAsync(customer, ct);
            }
        }

        return new CustomerDto
        {
            Id = customer.Id,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            PhoneNumber = customer.PhoneNumber,
            Email = customer.Email,
            Vehicles = customer.Vehicles.Select(v => new VehicleDto
            {
                VehicleNumber = v.VehicleNumber,
                Brand = v.Brand,
                Model = v.Model,
                Year = v.Year
            }).ToList()
        };
    }
}