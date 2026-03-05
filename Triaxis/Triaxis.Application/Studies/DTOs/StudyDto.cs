namespace Triaxis.Application.Studies.DTOs;

public record StudyDto(
    Guid Id,
    Guid ClientId,
    string Name,
    string ProtocolNumber,
    string? Phase,
    string Status,
    StudySettingsDto Settings,
    DateTime CreatedAt,
    DateTime? UpdatedAt);
