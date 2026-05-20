namespace Partpurja.Application.DTOs.ServiceReview
{
    public class CreateServiceReviewDto
    {
        public int CustomerId { get; set; }

        public int VehicleId { get; set; }

        public int Rating { get; set; }

        public string Comment { get; set; } = string.Empty;

        public string Comments { get; set; } = string.Empty;
    }
}