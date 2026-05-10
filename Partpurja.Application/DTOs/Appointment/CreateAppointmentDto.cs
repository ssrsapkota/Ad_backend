namespace Partpurja.Application.DTOs.Appointment
{
    public class CreateAppointmentDto
    {
        public int CustomerId { get; set; }

        public int VehicleId { get; set; }

        public DateTime AppointmentDate { get; set; }

        public string ServiceDescription { get; set; } = string.Empty;
    }
}