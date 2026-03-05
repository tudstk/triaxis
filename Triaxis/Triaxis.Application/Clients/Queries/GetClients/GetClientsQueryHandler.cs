using MediatR;
using Microsoft.EntityFrameworkCore;
using Triaxis.Application.Clients.DTOs;
using Triaxis.Application.Common.Interfaces;
using Triaxis.Application.Common.Models;

namespace Triaxis.Application.Clients.Queries.GetClients;

public class GetClientsQueryHandler : IRequestHandler<GetClientsQuery, PagedResult<ClientDto>>
{
    private readonly IApplicationDbContext _context;

    public GetClientsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<ClientDto>> Handle(GetClientsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Clients.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.ToLower();
            query = query.Where(c => c.Name.ToLower().Contains(search) || c.Code.ToLower().Contains(search));
        }

        if (request.IsActive.HasValue)
            query = query.Where(c => c.IsActive == request.IsActive.Value);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(c => c.Name)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(c => new ClientDto(c.Id, c.Name, c.Code, c.IsActive, c.CreatedAt, c.UpdatedAt))
            .ToListAsync(cancellationToken);

        return new PagedResult<ClientDto>(items, totalCount, request.Page, request.PageSize);
    }
}
