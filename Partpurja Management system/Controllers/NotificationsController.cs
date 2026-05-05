using Microsoft.AspNetCore.Mvc;
using Partpurja.Application.DTOs.Notification;
using Partpurja.Application.Interface.IServices;

namespace Partpurja.Api.Controllers
{
    /// <summary>
    /// API controller for notification and alert operations.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        private readonly IStockMonitorService _stockMonitorService;
        private readonly ICreditReminderService _creditReminderService;

        public NotificationsController(
            INotificationService notificationService,
            IStockMonitorService stockMonitorService,
            ICreditReminderService creditReminderService)
        {
            _notificationService = notificationService;
            _stockMonitorService = stockMonitorService;
            _creditReminderService = creditReminderService;
        }

        /// <summary>
        /// Gets all notifications for admin users, newest first.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NotificationDto>>> GetAdminNotifications()
        {
            var notifications = await _notificationService.GetAdminNotificationsAsync();
            return Ok(notifications);
        }

        /// <summary>
        /// Marks a notification as read.
        /// </summary>
        [HttpPatch("{id}/read")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var success = await _notificationService.MarkAsReadAsync(id);
            if (!success)
                return NotFound(new { message = "Notification not found." });
            return NoContent();
        }

        /// <summary>
        /// Scans parts inventory and creates low-stock notifications for admins.
        /// </summary>
        [HttpPost("trigger-stock-check")]
        public async Task<IActionResult> TriggerStockCheck()
        {
            var count = await _stockMonitorService.CheckLowStockAsync();
            return Ok(new { message = $"{count} low-stock notification(s) created." });
        }

        /// <summary>
        /// Scans overdue credit invoices, creates reminder records, and emails customers.
        /// </summary>
        [HttpPost("trigger-credit-reminders")]
        public async Task<IActionResult> TriggerCreditReminders()
        {
            var count = await _creditReminderService.ProcessOverdueCreditsAsync();
            return Ok(new { message = $"{count} credit reminder email(s) sent." });
        }
    }
}
