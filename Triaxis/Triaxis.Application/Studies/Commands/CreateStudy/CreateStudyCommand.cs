using MediatR;
using Triaxis.Domain.Common;

namespace Triaxis.Application.Studies.Commands.CreateStudy;

public record CreateStudyCommand(
    Guid ClientId,
    string Name,
    string ProtocolNumber,
    string? Phase = null) : IRequest<Result<Guid>>;
