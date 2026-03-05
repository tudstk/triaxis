using MediatR;
using Triaxis.Application.Common.Models;
using Triaxis.Application.Clients.DTOs;

namespace Triaxis.Application.Clients.Queries.GetClients;

public record GetClientsQuery(
    string? Search = null,
    bool? IsActive = null,
    int Page = 1,
    int PageSize = 20) : IRequest<PagedResult<ClientDto>>;
