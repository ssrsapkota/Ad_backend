using Partpurja.Application.DTOs.Vendor;

namespace Partpurja.Application.Interface.IRepository
{
    /// <summary>
    /// Interface for vendor repository operations.
    /// </summary>
    public interface IVendorRepository
    {
        /// <summary>
        /// Gets all active vendors.
        /// </summary>
        /// <returns>A collection of active vendors.</returns>
        Task<IEnumerable<VendorDto>> GetAllAsync();

        /// <summary>
        /// Gets a vendor by ID.
        /// </summary>
        /// <param name="id">The vendor ID.</param>
        /// <returns>The vendor if found; otherwise null.</returns>
        Task<VendorDto?> GetByIdAsync(int id);

        /// <summary>
        /// Creates a new vendor.
        /// </summary>
        /// <param name="createVendorDto">The vendor data to create.</param>
        /// <returns>The created vendor.</returns>
        Task<VendorDto> CreateAsync(CreateVendorDto createVendorDto);

        /// <summary>
        /// Updates an existing vendor.
        /// </summary>
        /// <param name="id">The vendor ID.</param>
        /// <param name="updateVendorDto">The vendor data to update.</param>
        /// <returns>The updated vendor if found; otherwise null.</returns>
        Task<VendorDto?> UpdateAsync(int id, UpdateVendorDto updateVendorDto);

        /// <summary>
        /// Soft deletes a vendor by setting IsActive to false.
        /// </summary>
        /// <param name="id">The vendor ID.</param>
        /// <returns>True if the vendor was deleted; otherwise false.</returns>
        Task<bool> DeleteAsync(int id);
    }
}
