namespace Partpurja.Application.DTOs.UnavailablePartRequest
{
    public class CreateUnavailablePartRequestDto
    {
        public int CustomerId { get; set; }

        public string PartName { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
    }
}