namespace Triaxis.Application.Clients.DTOs;

public record ClientDto(
    Guid Id,
    string Name,
    string Code,
    bool IsActive,
    DateTime CreatedAt,
    DateTime? UpdatedAt);
