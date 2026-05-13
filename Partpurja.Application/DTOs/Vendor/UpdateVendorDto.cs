namespace Partpurja.Application.DTOs.Vendor
{
    /// <summary>
    /// Data transfer object for updating an existing vendor.
    /// </summary>
    public class UpdateVendorDto
    {
        /// <summary>
        /// Name of the vendor.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Contact information for the vendor.
        /// </summary>
        public string Contact { get; set; } = string.Empty;

        /// <summary>
        /// Email address of the vendor.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Address of the vendor.
        /// </summary>
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// Indicates whether the vendor is active.
        /// </summary>
        public bool IsActive { get; set; }
    }
}
