using MediatR;
using Triaxis.Application.Auth.DTOs;
using Triaxis.Domain.Common;

namespace Triaxis.Application.Auth.Commands.Login;

public record LoginCommand(string Email, string Password) : IRequest<Result<AuthResponseDto>>;
