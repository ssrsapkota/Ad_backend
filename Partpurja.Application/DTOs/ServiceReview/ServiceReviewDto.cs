namespace Partpurja.Application.DTOs.ServiceReview
{
    public class ServiceReviewDto
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public int VehicleId { get; set; }

        public int Rating { get; set; }

        public string Comment { get; set; } = string.Empty;

        public string Comments => Comment;

        public string Status => "Published";

        public DateTime CreatedAt { get; set; }
    }
}