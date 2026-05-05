using Partpurja.Application.DTOs.Notification;
using Partpurja.Application.Interface.IRepository;
using Partpurja.Application.Interface.IServices;

namespace Partpurja.Infrastructure.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationService(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<IEnumerable<NotificationDto>> GetAdminNotificationsAsync()
        {
            return await _notificationRepository.GetAdminNotificationsAsync();
        }

        public async Task<bool> MarkAsReadAsync(int id)
        {
            return await _notificationRepository.MarkAsReadAsync(id);
        }
    }
}
