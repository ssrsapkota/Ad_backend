namespace Partpurja.Application.DTOs.History
{
public class ServiceHistoryDto
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string Status { get; set; } = string.Empty;
}
}