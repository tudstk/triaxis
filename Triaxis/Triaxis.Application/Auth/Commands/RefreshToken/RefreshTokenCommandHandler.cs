using MediatR;
using Microsoft.EntityFrameworkCore;
using Triaxis.Application.Auth.DTOs;
using Triaxis.Application.Common.Interfaces;
using Triaxis.Domain.Common;

namespace Triaxis.Application.Auth.Commands.RefreshToken;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, Result<AuthResponseDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;
    private readonly IJwtTokenService _jwtTokenService;

    public RefreshTokenCommandHandler(
        IApplicationDbContext context,
        IIdentityService identityService,
        IJwtTokenService jwtTokenService)
    {
        _context = context;
        _identityService = identityService;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<Result<AuthResponseDto>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var storedToken = await _context.RefreshTokens
            .FirstOrDefaultAsync(t => t.Token == request.RefreshToken, cancellationToken);

        if (storedToken is null || !storedToken.IsActive)
            return Result<AuthResponseDto>.Failure("Invalid or expired refresh token.");

        storedToken.RevokedAt = DateTime.UtcNow;

        var userInfo = await _identityService.GetUserInfoAsync(storedToken.UserId);
        if (userInfo is null)
            return Result<AuthResponseDto>.Failure("User not found.");

        var (email, clientId) = userInfo.Value;
        var roles = await _identityService.GetUserRolesAsync(storedToken.UserId);

        var accessToken = _jwtTokenService.GenerateAccessToken(storedToken.UserId, email, roles, clientId);
        var newRefreshTokenValue = _jwtTokenService.GenerateRefreshToken();

        var newRefreshToken = new Domain.Entities.RefreshToken
        {
            UserId = storedToken.UserId,
            Token = newRefreshTokenValue,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            CreatedAt = DateTime.UtcNow
        };

        _context.RefreshTokens.Add(newRefreshToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<AuthResponseDto>.Success(new AuthResponseDto(
            accessToken,
            newRefreshTokenValue,
            newRefreshToken.ExpiresAt));
    }
}
