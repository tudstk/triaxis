using MediatR;
using Triaxis.Application.Clients.DTOs;

namespace Triaxis.Application.Clients.Queries.GetClientById;

public record GetClientByIdQuery(Guid Id) : IRequest<ClientDto?>;
