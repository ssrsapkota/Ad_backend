namespace Partpurja.Application.DTOs.Customer
{
    public class UpdateCustomerDto
    {
        // Full name of Customer
        public string FullName { get; set; } = string.Empty;
        // Phone number of Customer
        public string Phone { get; set; } = string.Empty;
        // Address of Customer
        public string Address { get; set; } = string.Empty;
    }
}