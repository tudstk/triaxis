using MediatR;
using Microsoft.EntityFrameworkCore;
using Triaxis.Application.Common.Interfaces;
using Triaxis.Domain.Common;

namespace Triaxis.Application.Studies.Commands.UpdateStudy;

public class UpdateStudyCommandHandler : IRequestHandler<UpdateStudyCommand, Result<Guid>>
{
    private readonly IApplicationDbContext _context;

    public UpdateStudyCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Guid>> Handle(UpdateStudyCommand request, CancellationToken cancellationToken)
    {
        var study = await _context.Studies.FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);
        if (study is null)
            return Result<Guid>.Failure($"Study with ID '{request.Id}' was not found.");

        var duplicateProtocol = await _context.Studies
            .AnyAsync(s => s.ClientId == study.ClientId && s.ProtocolNumber == request.ProtocolNumber && s.Id != request.Id, cancellationToken);
        if (duplicateProtocol)
            return Result<Guid>.Failure($"A study with protocol number '{request.ProtocolNumber}' already exists for this client.");

        study.UpdateDetails(request.Name, request.ProtocolNumber, request.Phase);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(study.Id);
    }
}
