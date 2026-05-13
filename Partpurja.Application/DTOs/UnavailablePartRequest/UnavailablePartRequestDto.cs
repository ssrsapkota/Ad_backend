namespace Partpurja.Application.DTOs.UnavailablePartRequest
{
    public class UnavailablePartRequestDto
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public string PartName { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public DateTime RequestedAt { get; set; }
    }
}