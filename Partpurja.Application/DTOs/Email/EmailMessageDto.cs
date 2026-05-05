namespace Partpurja.Application.DTOs.Email
{
    /// <summary>
    /// Represents an outgoing email message.
    /// </summary>
    public class EmailMessageDto
    {
        public string To { get; set; } = string.Empty;
        public string ToName { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty; // HTML body
    }
}