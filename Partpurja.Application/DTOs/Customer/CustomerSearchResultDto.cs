namespace Partpurja.Application.DTOs.Customer
{
    public class CustomerSearchResultDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public List<string> VehicleRegistrationNumbers { get; set; } = new();
    }
}
