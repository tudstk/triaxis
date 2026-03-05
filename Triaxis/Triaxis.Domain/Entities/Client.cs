using Triaxis.Domain.Common;

namespace Triaxis.Domain.Entities;

public class Client : BaseEntity, ISoftDelete
{
    public string Name { get; private set; } = string.Empty;
    public string Code { get; private set; } = string.Empty;
    public bool IsActive { get; private set; } = true;
    public DateTime? DeletedAt { get; set; }
    public Guid? DeletedByUserId { get; set; }

    public ICollection<Study> Studies { get; private set; } = [];

    private Client() { }

    public static Client Create(string name, string code)
    {
        return new Client
        {
            Name = name,
            Code = code.ToUpperInvariant(),
            CreatedAt = DateTime.UtcNow
        };
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateDetails(string name)
    {
        Name = name;
        UpdatedAt = DateTime.UtcNow;
    }
}
