using MediatR;
using Microsoft.EntityFrameworkCore;
using Triaxis.Application.Common.Interfaces;
using Triaxis.Domain.Common;

namespace Triaxis.Application.Studies.Commands.ActivateStudy;

public class ActivateStudyCommandHandler : IRequestHandler<ActivateStudyCommand, Result<Guid>>
{
    private readonly IApplicationDbContext _context;

    public ActivateStudyCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Guid>> Handle(ActivateStudyCommand request, CancellationToken cancellationToken)
    {
        var study = await _context.Studies.FirstOrDefaultAsync(s => s.Id == request.StudyId, cancellationToken);
        if (study is null)
            return Result<Guid>.Failure($"Study with ID '{request.StudyId}' was not found.");

        var result = study.Activate();
        if (!result.IsSuccess)
            return Result<Guid>.Failure(result.Error!);

        await _context.SaveChangesAsync(cancellationToken);
        return Result<Guid>.Success(study.Id);
    }
}
