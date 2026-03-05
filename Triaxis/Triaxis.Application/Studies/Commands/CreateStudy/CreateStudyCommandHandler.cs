using MediatR;
using Microsoft.EntityFrameworkCore;
using Triaxis.Application.Common.Interfaces;
using Triaxis.Domain.Common;
using Triaxis.Domain.Entities;

namespace Triaxis.Application.Studies.Commands.CreateStudy;

public class CreateStudyCommandHandler : IRequestHandler<CreateStudyCommand, Result<Guid>>
{
    private readonly IApplicationDbContext _context;

    public CreateStudyCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Guid>> Handle(CreateStudyCommand request, CancellationToken cancellationToken)
    {
        var clientExists = await _context.Clients.AnyAsync(c => c.Id == request.ClientId, cancellationToken);
        if (!clientExists)
            return Result<Guid>.Failure($"Client with ID '{request.ClientId}' was not found.");

        var duplicateProtocol = await _context.Studies
            .AnyAsync(s => s.ClientId == request.ClientId && s.ProtocolNumber == request.ProtocolNumber, cancellationToken);
        if (duplicateProtocol)
            return Result<Guid>.Failure($"A study with protocol number '{request.ProtocolNumber}' already exists for this client.");

        var study = Study.Create(request.ClientId, request.Name, request.ProtocolNumber, request.Phase);
        _context.Studies.Add(study);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(study.Id);
    }
}
