namespace Partpurja.Application.DTOs.UnavailablePartRequest
{
    public class UnavailablePartRequestDto
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public string RequestedPartName { get; set; } = string.Empty;

        public int Quantity { get; set; }

        public string Notes { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }
}