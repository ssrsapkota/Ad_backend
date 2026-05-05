namespace Partpurja.Application.DTOs.Staff
{
    public class UpdateStaffDto
    {
        public string FullName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
