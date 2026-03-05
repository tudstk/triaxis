using MediatR;
using Microsoft.EntityFrameworkCore;
using Triaxis.Application.Common.Interfaces;
using Triaxis.Domain.Common;

namespace Triaxis.Application.Clients.Commands.UpdateClient;

public class UpdateClientCommandHandler : IRequestHandler<UpdateClientCommand, Result<Guid>>
{
    private readonly IApplicationDbContext _context;

    public UpdateClientCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Guid>> Handle(UpdateClientCommand request, CancellationToken cancellationToken)
    {
        var client = await _context.Clients.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
        if (client is null)
            return Result<Guid>.Failure($"Client with ID '{request.Id}' was not found.");

        client.UpdateDetails(request.Name);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(client.Id);
    }
}
