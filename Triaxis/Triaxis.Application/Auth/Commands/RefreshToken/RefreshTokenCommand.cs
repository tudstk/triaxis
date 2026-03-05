using MediatR;
using Triaxis.Application.Auth.DTOs;
using Triaxis.Domain.Common;

namespace Triaxis.Application.Auth.Commands.RefreshToken;

public record RefreshTokenCommand(string RefreshToken) : IRequest<Result<AuthResponseDto>>;
