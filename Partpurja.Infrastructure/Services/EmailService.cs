using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using Partpurja.Application.DTOs.Email;
using Partpurja.Application.Interface.IServices;

namespace Partpurja.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IOptions<EmailSettings> settings, ILogger<EmailService> logger)
        {
            _settings = settings.Value;
            _logger = logger;
        }

        public async Task<bool> SendEmailAsync(EmailMessageDto message)
        {
            try
            {
                var email = new MimeMessage();
                email.From.Add(new MailboxAddress(_settings.SenderName, _settings.SenderEmail));
                email.To.Add(new MailboxAddress(message.ToName, message.To));
                email.Subject = message.Subject;
                email.Body = new BodyBuilder { HtmlBody = message.Body }.ToMessageBody();

                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(
                    _settings.SmtpServer,
                    _settings.Port,
                    _settings.UseSsl ? SecureSocketOptions.StartTls : SecureSocketOptions.None);
                await smtp.AuthenticateAsync(_settings.Username, _settings.Password);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email to {To}", message.To);
                return false;
            }
        }
    }
}
