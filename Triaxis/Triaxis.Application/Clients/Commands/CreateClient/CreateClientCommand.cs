using MediatR;
using Triaxis.Domain.Common;

namespace Triaxis.Application.Clients.Commands.CreateClient;

public record CreateClientCommand(string Name, string Code) : IRequest<Result<Guid>>;
