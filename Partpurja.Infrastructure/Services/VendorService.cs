using Partpurja.Application.DTOs.Vendor;
using Partpurja.Application.Interface.IRepository;
using Partpurja.Application.Interface.IServices;

namespace Partpurja.Infrastructure.Services
{
    /// <summary>
    /// Service for vendor business logic operations.
    /// </summary>
    public class VendorService : IVendorService
    {
        private readonly IVendorRepository _vendorRepository;

        public VendorService(IVendorRepository vendorRepository)
        {
            _vendorRepository = vendorRepository;
        }

        /// <summary>
        /// Gets all active vendors.
        /// </summary>
        public async Task<IEnumerable<VendorDto>> GetAllVendorsAsync()
        {
            return await _vendorRepository.GetAllAsync();
        }

        /// <summary>
        /// Gets a vendor by ID.
        /// </summary>
        public async Task<VendorDto?> GetVendorByIdAsync(int id)
        {
            return await _vendorRepository.GetByIdAsync(id);
        }

        /// <summary>
        /// Creates a new vendor.
        /// </summary>
        public async Task<VendorDto> CreateVendorAsync(CreateVendorDto createVendorDto)
        {
            return await _vendorRepository.CreateAsync(createVendorDto);
        }

        /// <summary>
        /// Updates an existing vendor.
        /// </summary>
        public async Task<VendorDto?> UpdateVendorAsync(int id, UpdateVendorDto updateVendorDto)
        {
            return await _vendorRepository.UpdateAsync(id, updateVendorDto);
        }

        /// <summary>
        /// Soft deletes a vendor.
        /// </summary>
        public async Task<bool> DeleteVendorAsync(int id)
        {
            return await _vendorRepository.DeleteAsync(id);
        }
    }
}
