using Partpurja.Application.DTOs.Notification;

namespace Partpurja.Application.Interface.IServices
{
    /// <summary>
    /// Interface for notification business logic.
    /// </summary>
    public interface INotificationService
    {
        /// <summary>
        /// Gets all notifications addressed to admin users.
        /// </summary>
        Task<IEnumerable<NotificationDto>> GetAdminNotificationsAsync();

        /// <summary>
        /// Marks a notification as read.
        /// </summary>
        Task<bool> MarkAsReadAsync(int id);
    }
}