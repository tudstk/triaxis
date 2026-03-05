using MediatR;
using Triaxis.Domain.Common;

namespace Triaxis.Application.Clients.Commands.UpdateClient;

public record UpdateClientCommand(Guid Id, string Name) : IRequest<Result<Guid>>;
