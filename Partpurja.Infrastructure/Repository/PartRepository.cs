using Microsoft.EntityFrameworkCore;
using Partpurja.Application.Interface.IRepository;
using Partpurja.Domain.Models;
using Partpurja.Infrastructure.Persistence;

namespace Partpurja.Infrastructure.Repository
{
    /// <summary>
    /// Part repository — feature 15 only needs the low-stock query.
    /// Full CRUD will be added by feature 03.
    /// </summary>
    public class PartRepository : IPartRepository
    {
        private readonly AppDbContext _context;

        public PartRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Part>> GetLowStockPartsAsync()
        {
            return await _context.Parts
                .Where(p => p.IsActive && p.Stock < p.ReorderLevel)
                .OrderBy(p => p.Stock)
                .ToListAsync();
        }
    }
}