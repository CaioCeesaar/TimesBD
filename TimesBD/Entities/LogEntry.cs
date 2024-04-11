namespace TimesBD.Entities;

public class LogEntry
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string Action { get; set; }
    public string Message { get; set; }    
    public string Details { get; set; }
}