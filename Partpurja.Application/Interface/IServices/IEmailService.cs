using Partpurja.Application.DTOs.Email;

namespace Partpurja.Application.Interface.IServices
{
    /// <summary>
    /// Interface for sending emails over SMTP.
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Sends an email asynchronously.
        /// Returns true on success, false on failure (logged, not thrown).
        /// </summary>
        Task<bool> SendEmailAsync(EmailMessageDto message);
    }
}