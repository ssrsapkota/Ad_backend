namespace Partpurja.Application.DTOs.Email
{
    /// <summary>
    /// SMTP configuration bound from the "EmailSettings" section of appsettings.json.
    /// </summary>
    public class EmailSettings
    {
        public string SmtpServer { get; set; } = string.Empty;     // e.g. "smtp.gmail.com"
        public int Port { get; set; }                              // e.g. 587
        public string SenderName { get; set; } = string.Empty;     // "Partpurja Management System"
        public string SenderEmail { get; set; } = string.Empty;    // your gmail
        public string Username { get; set; } = string.Empty;       // usually same as SenderEmail
        public string Password { get; set; } = string.Empty;       // Gmail app password
        public bool UseSsl { get; set; } = true;
    }
}