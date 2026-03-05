using MediatR;
using Microsoft.EntityFrameworkCore;
using Triaxis.Application.Common.Interfaces;
using Triaxis.Domain.Common;
using Triaxis.Domain.Entities;

namespace Triaxis.Application.Clients.Commands.CreateClient;

public class CreateClientCommandHandler : IRequestHandler<CreateClientCommand, Result<Guid>>
{
    private readonly IApplicationDbContext _context;

    public CreateClientCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Guid>> Handle(CreateClientCommand request, CancellationToken cancellationToken)
    {
        var codeUpper = request.Code.ToUpperInvariant();
        var exists = await _context.Clients.AnyAsync(c => c.Code == codeUpper, cancellationToken);
        if (exists)
            return Result<Guid>.Failure($"A client with code '{codeUpper}' already exists.");

        var client = Client.Create(request.Name, request.Code);
        _context.Clients.Add(client);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(client.Id);
    }
}
