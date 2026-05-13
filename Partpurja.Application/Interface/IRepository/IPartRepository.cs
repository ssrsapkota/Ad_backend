using Partpurja.Domain.Models;

namespace Partpurja.Application.Interface.IRepository
{
    /// <summary>
    /// Interface for part persistence operations.
    /// Feature 15 only needs the low-stock query. Full CRUD belongs to feature 03.
    /// </summary>
    public interface IPartRepository
    {
        /// <summary>
        /// Gets all active parts whose stock has fallen below their reorder level.
        /// </summary>
        Task<IEnumerable<Part>> GetLowStockPartsAsync();
    }
}