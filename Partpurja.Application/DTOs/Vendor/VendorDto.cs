namespace Partpurja.Application.DTOs.Vendor
{
    /// <summary>
    /// Data transfer object for reading vendor information.
    /// </summary>
    public class VendorDto
    {
        /// <summary>
        /// Unique identifier for the vendor.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the vendor.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Contact information for the vendor.
        /// </summary>
        public string Contact { get; set; } = string.Empty;

        /// <summary>
        /// Address of the vendor.
        /// </summary>
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// Indicates whether the vendor is active.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Date and time when the vendor was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
}
