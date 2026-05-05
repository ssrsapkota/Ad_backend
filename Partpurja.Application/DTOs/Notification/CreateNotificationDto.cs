namespace Partpurja.Application.DTOs.Notification
{
    /// <summary>
    /// Data transfer object for creating a new notification.
    /// </summary>
    public class CreateNotificationDto
    {
        public int UserId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public string Type { get; set; } = string.Empty;
    }
}