namespace Triaxis.Application.Common.Interfaces;

public interface ICurrentUser
{
    Guid UserId { get; }
    Guid? ClientId { get; }
    Guid? StudyId { get; }
    string? IpAddress { get; }
    IReadOnlyList<string> Roles { get; }
}
