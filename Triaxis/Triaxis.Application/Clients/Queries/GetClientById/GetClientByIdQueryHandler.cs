using MediatR;
using Microsoft.EntityFrameworkCore;
using Triaxis.Application.Clients.DTOs;
using Triaxis.Application.Common.Interfaces;

namespace Triaxis.Application.Clients.Queries.GetClientById;

public class GetClientByIdQueryHandler : IRequestHandler<GetClientByIdQuery, ClientDto?>
{
    private readonly IApplicationDbContext _context;

    public GetClientByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ClientDto?> Handle(GetClientByIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.Clients
            .AsNoTracking()
            .Where(c => c.Id == request.Id)
            .Select(c => new ClientDto(c.Id, c.Name, c.Code, c.IsActive, c.CreatedAt, c.UpdatedAt))
            .FirstOrDefaultAsync(cancellationToken);
    }
}
