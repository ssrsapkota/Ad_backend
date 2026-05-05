namespace Partpurja.Application.DTOs.Staff
{
    public class CreateStaffDto
    {
        public int UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
    }
}
