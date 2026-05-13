using Microsoft.EntityFrameworkCore;
using Partpurja.Application.DTOs.Notification;
using Partpurja.Application.Interface.IRepository;
using Partpurja.Domain.Models;
using Partpurja.Infrastructure.Persistence;

namespace Partpurja.Infrastructure.Repository
{
    /// <summary>
    /// Repository for notification database operations.
    /// </summary>
    public class NotificationRepository : INotificationRepository
    {
        private const string AdminRoleName = "Admin";
        private const string LowStockType = "LowStock";

        private readonly AppDbContext _context;

        public NotificationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<NotificationDto>> GetAdminNotificationsAsync()
        {
            var notifications = await _context.Notifications
                .Include(n => n.User)
                    .ThenInclude(u => u!.Role)
                .Where(n => n.User != null
                            && n.User.Role != null
                            && n.User.Role.Name == AdminRoleName)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();

            return notifications.Select(MapToDto);
        }

        public async Task<NotificationDto> CreateAsync(CreateNotificationDto createNotificationDto)
        {
            var notification = new Notification
            {
                UserId = createNotificationDto.UserId,
                Title = createNotificationDto.Title,
                Message = createNotificationDto.Message,
                Type = createNotificationDto.Type,
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();

            return MapToDto(notification);
        }

        public async Task<bool> MarkAsReadAsync(int id)
        {
            var notification = await _context.Notifications.FindAsync(id);

            if (notification == null)
            {
                return false;
            }

            notification.IsRead = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> LowStockNotificationExistsAsync(int partId)
        {
            var tag = $"[PartId:{partId}]";

            return await _context.Notifications
                .AnyAsync(n => n.Type == LowStockType
                               && !n.IsRead
                               && n.Message.Contains(tag));
        }

        public async Task<IEnumerable<int>> GetAdminUserIdsAsync()
        {
            return await _context.Users
                .Where(u => u.IsActive
                            && u.Role != null
                            && u.Role.Name == AdminRoleName)
                .Select(u => u.Id)
                .ToListAsync();
        }

        private NotificationDto MapToDto(Notification notification)
        {
            return new NotificationDto
            {
                Id = notification.Id,
                UserId = notification.UserId,
                Title = notification.Title,
                Message = notification.Message,
                Type = notification.Type,
                IsRead = notification.IsRead,
                CreatedAt = notification.CreatedAt
            };
        }
    }
}