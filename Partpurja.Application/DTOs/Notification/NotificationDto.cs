namespace Partpurja.Application.DTOs.Notification
{
    /// <summary>
    /// Data transfer object for reading notification information.
    /// </summary>
    public class NotificationDto
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public string Type { get; set; } = string.Empty; // "LowStock", "CreditReminder"

        public bool IsRead { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}