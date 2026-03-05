using MediatR;
using Microsoft.EntityFrameworkCore;
using Triaxis.Application.Common.Interfaces;
using Triaxis.Application.Common.Models;
using Triaxis.Application.Studies.DTOs;

namespace Triaxis.Application.Studies.Queries.GetStudiesByClient;

public class GetStudiesByClientQueryHandler : IRequestHandler<GetStudiesByClientQuery, PagedResult<StudyDto>>
{
    private readonly IApplicationDbContext _context;

    public GetStudiesByClientQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<StudyDto>> Handle(GetStudiesByClientQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Studies
            .AsNoTracking()
            .Where(s => s.ClientId == request.ClientId);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.ToLower();
            query = query.Where(s => s.Name.ToLower().Contains(search) || s.ProtocolNumber.ToLower().Contains(search));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(s => s.Name)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(s => new StudyDto(
                s.Id,
                s.ClientId,
                s.Name,
                s.ProtocolNumber,
                s.Phase,
                s.Status.ToString(),
                new StudySettingsDto(
                    s.Settings.EnableRandomization,
                    s.Settings.EnableDoseManagement,
                    s.Settings.EnableCohorts,
                    s.Settings.EnableDrugManagement,
                    s.Settings.EnableUnblinding,
                    s.Settings.AllowRescreening,
                    s.Settings.RequireWeightAtVisit,
                    s.Settings.SubjectNumberFormat,
                    s.Settings.SubjectNumberLength,
                    s.Settings.SubjectNumberAssignmentTrigger),
                s.CreatedAt,
                s.UpdatedAt))
            .ToListAsync(cancellationToken);

        return new PagedResult<StudyDto>(items, totalCount, request.Page, request.PageSize);
    }
}
