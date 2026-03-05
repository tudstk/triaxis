using MediatR;
using Microsoft.EntityFrameworkCore;
using Triaxis.Application.Common.Interfaces;
using Triaxis.Domain.Common;

namespace Triaxis.Application.Clients.Commands.DeactivateClient;

public class DeactivateClientCommandHandler : IRequestHandler<DeactivateClientCommand, Result<Guid>>
{
    private readonly IApplicationDbContext _context;

    public DeactivateClientCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Guid>> Handle(DeactivateClientCommand request, CancellationToken cancellationToken)
    {
        var client = await _context.Clients.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
        if (client is null)
            return Result<Guid>.Failure($"Client with ID '{request.Id}' was not found.");

        client.Deactivate();
        await _context.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(client.Id);
    }
}
