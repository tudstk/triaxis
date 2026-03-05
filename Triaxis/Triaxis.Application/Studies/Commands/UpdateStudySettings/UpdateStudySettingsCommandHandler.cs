using MediatR;
using Microsoft.EntityFrameworkCore;
using Triaxis.Application.Common.Interfaces;
using Triaxis.Domain.Common;

namespace Triaxis.Application.Studies.Commands.UpdateStudySettings;

public class UpdateStudySettingsCommandHandler : IRequestHandler<UpdateStudySettingsCommand, Result<Guid>>
{
    private readonly IApplicationDbContext _context;

    public UpdateStudySettingsCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Guid>> Handle(UpdateStudySettingsCommand request, CancellationToken cancellationToken)
    {
        var study = await _context.Studies.FirstOrDefaultAsync(s => s.Id == request.StudyId, cancellationToken);
        if (study is null)
            return Result<Guid>.Failure($"Study with ID '{request.StudyId}' was not found.");

        var result = study.UpdateSettings(request.Settings);
        if (!result.IsSuccess)
            return Result<Guid>.Failure(result.Error!);

        await _context.SaveChangesAsync(cancellationToken);
        return Result<Guid>.Success(study.Id);
    }
}
