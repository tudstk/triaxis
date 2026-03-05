using MediatR;
using Triaxis.Application.Studies.DTOs;

namespace Triaxis.Application.Studies.Queries.GetStudyById;

public record GetStudyByIdQuery(Guid Id) : IRequest<StudyDto?>;
