namespace Triaxis.Domain.Entities;

public class AuditLog
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string EntityType { get; set; }
    public required string EntityId { get; set; }
    public required string Action { get; set; }
    public string? Changes { get; set; }
    public Guid? UserId { get; set; }
    public DateTime Timestamp { get; set; }
    public string? IpAddress { get; set; }
}
