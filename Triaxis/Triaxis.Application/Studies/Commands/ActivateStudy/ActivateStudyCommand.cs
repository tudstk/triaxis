using MediatR;
using Triaxis.Domain.Common;

namespace Triaxis.Application.Studies.Commands.ActivateStudy;

public record ActivateStudyCommand(Guid StudyId) : IRequest<Result<Guid>>;
