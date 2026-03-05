using MediatR;
using Microsoft.EntityFrameworkCore;
using Triaxis.Application.Common.Interfaces;
using Triaxis.Application.Studies.DTOs;

namespace Triaxis.Application.Studies.Queries.GetStudyById;

public class GetStudyByIdQueryHandler : IRequestHandler<GetStudyByIdQuery, StudyDto?>
{
    private readonly IApplicationDbContext _context;

    public GetStudyByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<StudyDto?> Handle(GetStudyByIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.Studies
            .AsNoTracking()
            .Where(s => s.Id == request.Id)
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
            .FirstOrDefaultAsync(cancellationToken);
    }
}
