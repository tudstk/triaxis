using MediatR;
using Triaxis.Application.Common.Models;
using Triaxis.Application.Studies.DTOs;

namespace Triaxis.Application.Studies.Queries.GetStudiesByClient;

public record GetStudiesByClientQuery(
    Guid ClientId,
    string? Search = null,
    int Page = 1,
    int PageSize = 20) : IRequest<PagedResult<StudyDto>>;
