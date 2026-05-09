using Partpurja.Application.DTOs.Customers;
using Partpurja.Application.DTOs.Search;
using Partpurja.Application.Interface.IRepository;
using Partpurja.Application.Interface.IServices;

namespace Partpurja.Application.Services;

public class SearchService : ISearchService
{
    private readonly ICustomerRepository _customerRepository;

    public SearchService(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<CustomerSearchResultDto> SearchCustomersAsync(CustomerSearchRequestDto request, CancellationToken ct = default)
    {
        var customers = await _customerRepository.SearchAsync(request.SearchTerm, ct);

        return new CustomerSearchResultDto
        {
            Customers = customers.Select(c => new CustomerDto
            {
                Id = c.Id,
                FirstName = c.FirstName,
                LastName = c.LastName,
                PhoneNumber = c.PhoneNumber,
                Email = c.Email,
                Vehicles = c.Vehicles.Select(v => new VehicleDto
                {
                    VehicleNumber = v.VehicleNumber,
                    Brand = v.Brand,
                    Model = v.Model,
                    Year = v.Year
                }).ToList()
            }).ToList()
        };
    }
}