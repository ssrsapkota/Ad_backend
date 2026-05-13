using Partpurja.Application.DTOs.Customers;

namespace Partpurja.Application.DTOs.Search;

public class CustomerSearchResultDto
{
    public List<CustomerDto> Customers { get; set; } = new();
}