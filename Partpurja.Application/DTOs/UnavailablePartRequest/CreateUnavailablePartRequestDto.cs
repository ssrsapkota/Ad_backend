namespace Partpurja.Application.DTOs.UnavailablePartRequest
{
    public class CreateUnavailablePartRequestDto
    {
        public int CustomerId { get; set; }

        public string RequestedPartName { get; set; } = string.Empty;

        public int Quantity { get; set; }

        public string Notes { get; set; } = string.Empty;
    }
}