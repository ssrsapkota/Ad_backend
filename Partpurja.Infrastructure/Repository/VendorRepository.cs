using Microsoft.EntityFrameworkCore;
using Partpurja.Application.DTOs.Vendor;
using Partpurja.Application.Interface.IRepository;
using Partpurja.Domain.Models;
using Partpurja.Infrastructure.Persistence;

namespace Partpurja.Infrastructure.Repository
{
    /// <summary>
    /// Repository for vendor database operations.
    /// </summary>
    public class VendorRepository : IVendorRepository
    {
        private readonly AppDbContext _context;

        public VendorRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all active vendors.
        /// </summary>
        public async Task<IEnumerable<VendorDto>> GetAllAsync()
        {
            var vendors = await _context.Vendors
                .Where(v => v.IsActive)
                .ToListAsync();

            return vendors.Select(MapToDto);
        }

        /// <summary>
        /// Gets a vendor by ID.
        /// </summary>
        public async Task<VendorDto?> GetByIdAsync(int id)
        {
            var vendor = await _context.Vendors.FindAsync(id);

            if (vendor == null || !vendor.IsActive)
            {
                return null;
            }

            return MapToDto(vendor);
        }

        /// <summary>
        /// Creates a new vendor.
        /// </summary>
        public async Task<VendorDto> CreateAsync(CreateVendorDto createVendorDto)
        {
            var vendor = new Vendor
            {
                Name = createVendorDto.Name,
                Contact = createVendorDto.Contact,
                Address = createVendorDto.Address,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.Vendors.Add(vendor);
            await _context.SaveChangesAsync();

            return MapToDto(vendor);
        }

        /// <summary>
        /// Updates an existing vendor.
        /// </summary>
        public async Task<VendorDto?> UpdateAsync(int id, UpdateVendorDto updateVendorDto)
        {
            var vendor = await _context.Vendors.FindAsync(id);

            if (vendor == null || !vendor.IsActive)
            {
                return null;
            }

            vendor.Name = updateVendorDto.Name;
            vendor.Contact = updateVendorDto.Contact;
            vendor.Address = updateVendorDto.Address;
            vendor.IsActive = updateVendorDto.IsActive;

            _context.Vendors.Update(vendor);
            await _context.SaveChangesAsync();

            return MapToDto(vendor);
        }

        /// <summary>
        /// Soft deletes a vendor by setting IsActive to false.
        /// </summary>
        public async Task<bool> DeleteAsync(int id)
        {
            var vendor = await _context.Vendors.FindAsync(id);

            if (vendor == null || !vendor.IsActive)
            {
                return false;
            }

            vendor.IsActive = false;
            _context.Vendors.Update(vendor);
            await _context.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Maps a Vendor entity to VendorDto.
        /// </summary>
        private VendorDto MapToDto(Vendor vendor)
        {
            return new VendorDto
            {
                Id = vendor.Id,
                Name = vendor.Name,
                Contact = vendor.Contact,
                Address = vendor.Address,
                IsActive = vendor.IsActive,
                CreatedAt = vendor.CreatedAt
            };
        }
    }
}
