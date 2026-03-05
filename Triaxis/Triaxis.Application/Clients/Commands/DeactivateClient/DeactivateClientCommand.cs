using MediatR;
using Triaxis.Domain.Common;

namespace Triaxis.Application.Clients.Commands.DeactivateClient;

public record DeactivateClientCommand(Guid Id) : IRequest<Result<Guid>>;
