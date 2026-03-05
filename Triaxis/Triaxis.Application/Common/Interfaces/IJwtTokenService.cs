namespace Triaxis.Application.Common.Interfaces;

public interface IJwtTokenService
{
    string GenerateAccessToken(Guid userId, string email, IList<string> roles, Guid? clientId);
    string GenerateRefreshToken();
}
