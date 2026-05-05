using Microsoft.Extensions.Logging;
using Partpurja.Application.DTOs.Notification;
using Partpurja.Application.Interface.IRepository;
using Partpurja.Application.Interface.IServices;

namespace Partpurja.Infrastructure.Services
{
    public class StockMonitorService : IStockMonitorService
    {
        private const string LowStockNotificationType = "LowStock";

        private readonly IPartRepository _partRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly ILogger<StockMonitorService> _logger;

        public StockMonitorService(
            IPartRepository partRepository,
            INotificationRepository notificationRepository,
            ILogger<StockMonitorService> logger)
        {
            _partRepository = partRepository;
            _notificationRepository = notificationRepository;
            _logger = logger;
        }

        public async Task<int> CheckLowStockAsync()
        {
            // 1. Find low-stock parts
            var lowStockParts = (await _partRepository.GetLowStockPartsAsync()).ToList();

            if (lowStockParts.Count == 0)
            {
                _logger.LogInformation("Stock check completed. No parts below reorder level.");
                return 0;
            }

            // 2. Find admins to notify
            var adminUserIds = (await _notificationRepository.GetAdminUserIdsAsync()).ToList();

            if (adminUserIds.Count == 0)
            {
                _logger.LogWarning(
                    "Low stock detected on {Count} parts but no admin users to notify.",
                    lowStockParts.Count);
                return 0;
            }

            // 3. For each low-stock part, create a notification per admin (deduplicated)
            var created = 0;

            foreach (var part in lowStockParts)
            {
                // Skip if any admin already has an unread alert for this part
                var alreadyNotified = await _notificationRepository.LowStockNotificationExistsAsync(part.Id);
                if (alreadyNotified)
                {
                    continue;
                }

                foreach (var adminId in adminUserIds)
                {
                    var dto = new CreateNotificationDto
                    {
                        UserId = adminId,
                        Type = LowStockNotificationType,
                        Title = $"Low stock alert: {part.Name}",
                        Message = $"Part '{part.Name}' (Part No: {part.PartNumber}) " +
                                  $"is running low. Current stock: {part.Stock}, " +
                                  $"reorder level: {part.ReorderLevel}. [PartId:{part.Id}]"
                    };

                    await _notificationRepository.CreateAsync(dto);
                    created++;
                }
            }

            _logger.LogInformation(
                "Stock check completed. Created {Count} notifications across {Admins} admins.",
                created, adminUserIds.Count);

            return created;
        }
    }
}