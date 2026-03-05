using MediatR;
using Triaxis.Domain.Common;

namespace Triaxis.Application.Studies.Commands.UpdateStudy;

public record UpdateStudyCommand(
    Guid Id,
    string Name,
    string ProtocolNumber,
    string? Phase = null) : IRequest<Result<Guid>>;
