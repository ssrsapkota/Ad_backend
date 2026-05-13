using Partpurja.Application.DTOs.Vendor;

namespace Partpurja.Application.Interface.IServices
{
    /// <summary>
    /// Interface for vendor service operations.
    /// </summary>
    public interface IVendorService
    {
        /// <summary>
        /// Gets all active vendors.
        /// </summary>
        /// <returns>A collection of active vendors.</returns>
        Task<IEnumerable<VendorDto>> GetAllVendorsAsync();

        /// <summary>
        /// Gets a vendor by ID.
        /// </summary>
        /// <param name="id">The vendor ID.</param>
        /// <returns>The vendor if found; otherwise null.</returns>
        Task<VendorDto?> GetVendorByIdAsync(int id);

        /// <summary>
        /// Creates a new vendor.
        /// </summary>
        /// <param name="createVendorDto">The vendor data to create.</param>
        /// <returns>The created vendor.</returns>
        Task<VendorDto> CreateVendorAsync(CreateVendorDto createVendorDto);

        /// <summary>
        /// Updates an existing vendor.
        /// </summary>
        /// <param name="id">The vendor ID.</param>
        /// <param name="updateVendorDto">The vendor data to update.</param>
        /// <returns>The updated vendor if found; otherwise null.</returns>
        Task<VendorDto?> UpdateVendorAsync(int id, UpdateVendorDto updateVendorDto);

        /// <summary>
        /// Soft deletes a vendor.
        /// </summary>
        /// <param name="id">The vendor ID.</param>
        /// <returns>True if the vendor was deleted; otherwise false.</returns>
        Task<bool> DeleteVendorAsync(int id);
    }
}
