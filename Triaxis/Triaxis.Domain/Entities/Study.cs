using Triaxis.Domain.Common;
using Triaxis.Domain.Common.Enums;

namespace Triaxis.Domain.Entities;

public class Study : BaseEntity, IClientScoped, ISoftDelete
{
    public Guid ClientId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string ProtocolNumber { get; private set; } = string.Empty;
    public string? Phase { get; private set; }
    public StudyStatus Status { get; private set; } = StudyStatus.Draft;
    public StudySettingsData Settings { get; private set; } = StudySettingsData.WithDefaults();
    public DateTime? DeletedAt { get; set; }
    public Guid? DeletedByUserId { get; set; }

    public Client Client { get; private set; } = null!;

    private Study() { }

    public static Study Create(Guid clientId, string name, string protocolNumber, string? phase = null)
    {
        return new Study
        {
            ClientId = clientId,
            Name = name,
            ProtocolNumber = protocolNumber,
            Phase = phase,
            CreatedAt = DateTime.UtcNow
        };
    }

    public Result<bool> Activate()
    {
        if (Status != StudyStatus.Draft)
            return Result<bool>.Failure("Study can only be activated from Draft status.");
        Status = StudyStatus.Active;
        UpdatedAt = DateTime.UtcNow;
        return Result<bool>.Success(true);
    }

    public Result<bool> Complete()
    {
        if (Status != StudyStatus.Active)
            return Result<bool>.Failure("Study can only be completed from Active status.");
        Status = StudyStatus.Completed;
        UpdatedAt = DateTime.UtcNow;
        return Result<bool>.Success(true);
    }

    public Result<bool> Decommission()
    {
        if (Status != StudyStatus.Completed)
            return Result<bool>.Failure("Study can only be decommissioned from Completed status.");
        Status = StudyStatus.Decommissioned;
        UpdatedAt = DateTime.UtcNow;
        return Result<bool>.Success(true);
    }

    public void UpdateDetails(string name, string protocolNumber, string? phase)
    {
        Name = name;
        ProtocolNumber = protocolNumber;
        Phase = phase;
        UpdatedAt = DateTime.UtcNow;
    }

    public Result<bool> UpdateSettings(StudySettingsData settings)
    {
        if (Status == StudyStatus.Decommissioned)
            return Result<bool>.Failure("Cannot update settings on a decommissioned study.");
        Settings = settings;
        UpdatedAt = DateTime.UtcNow;
        return Result<bool>.Success(true);
    }
}
