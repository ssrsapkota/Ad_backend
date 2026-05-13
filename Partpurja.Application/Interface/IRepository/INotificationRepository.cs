using Partpurja.Application.DTOs.Notification;

namespace Partpurja.Application.Interface.IRepository
{
    /// <summary>
    /// Interface for notification persistence operations.
    /// </summary>
    public interface INotificationRepository
    {
        /// <summary>
        /// Gets all notifications addressed to admin users, newest first.
        /// </summary>
        Task<IEnumerable<NotificationDto>> GetAdminNotificationsAsync();

        /// <summary>
        /// Creates a new notification.
        /// </summary>
        Task<NotificationDto> CreateAsync(CreateNotificationDto createNotificationDto);

        /// <summary>
        /// Marks a notification as read.
        /// </summary>
        Task<bool> MarkAsReadAsync(int id);

        /// <summary>
        /// Checks if an unread low-stock notification already exists for a part.
        /// Used to avoid spamming admins with duplicate alerts.
        /// </summary>
        Task<bool> LowStockNotificationExistsAsync(int partId);

        /// <summary>
        /// Gets the IDs of all active admin users (so we know who to notify).
        /// </summary>
        Task<IEnumerable<int>> GetAdminUserIdsAsync();
    }
}