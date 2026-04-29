namespace Partpurja.Application.Interface.IServices
{
    /// <summary>
    /// Service for scanning parts inventory and notifying admins of low stock.
    /// </summary>
    public interface IStockMonitorService
    {
        /// <summary>
        /// Scans all active parts. For each one with stock below reorder level,
        /// creates a notification for every admin (deduplicated).
        /// Returns the number of new notifications created.
        /// </summary>
        Task<int> CheckLowStockAsync();
    }
}