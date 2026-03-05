using MediatR;
using Triaxis.Application.Auth.DTOs;
using Triaxis.Application.Common.Interfaces;
using Triaxis.Domain.Common;
using Triaxis.Domain.Entities;

namespace Triaxis.Application.Auth.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<AuthResponseDto>>
{
    private readonly IIdentityService _identityService;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IApplicationDbContext _context;

    public LoginCommandHandler(
        IIdentityService identityService,
        IJwtTokenService jwtTokenService,
        IApplicationDbContext context)
    {
        _identityService = identityService;
        _jwtTokenService = jwtTokenService;
        _context = context;
    }

    public async Task<Result<AuthResponseDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var (success, userId, email, clientId) = await _identityService.ValidateCredentialsAsync(request.Email, request.Password);
        if (!success)
            return Result<AuthResponseDto>.Failure("Invalid email or password.");

        var roles = await _identityService.GetUserRolesAsync(userId);
        var accessToken = _jwtTokenService.GenerateAccessToken(userId, email, roles, clientId);
        var refreshTokenValue = _jwtTokenService.GenerateRefreshToken();

        var refreshToken = new Domain.Entities.RefreshToken
        {
            UserId = userId,
            Token = refreshTokenValue,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            CreatedAt = DateTime.UtcNow
        };

        _context.RefreshTokens.Add(refreshToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<AuthResponseDto>.Success(new AuthResponseDto(
            accessToken,
            refreshTokenValue,
            refreshToken.ExpiresAt));
    }
}
