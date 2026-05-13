namespace Partpurja.Domain.Models.Users;

public class User
{
    public int Id { get; set; } // Matches SERIAL in PostgreSQL
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string? Email { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}