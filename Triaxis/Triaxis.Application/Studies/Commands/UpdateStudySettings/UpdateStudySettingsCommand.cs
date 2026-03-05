using MediatR;
using Triaxis.Domain.Common;
using Triaxis.Domain.Entities;

namespace Triaxis.Application.Studies.Commands.UpdateStudySettings;

public record UpdateStudySettingsCommand(Guid StudyId, StudySettingsData Settings) : IRequest<Result<Guid>>;
