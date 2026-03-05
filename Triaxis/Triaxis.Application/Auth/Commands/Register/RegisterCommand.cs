using MediatR;
using Triaxis.Domain.Common;

namespace Triaxis.Application.Auth.Commands.Register;

public record RegisterCommand(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    Guid? ClientId = null) : IRequest<Result<Guid>>;
