using Triaxis.Domain.Common;

namespace Triaxis.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<Result<Guid>> CreateUserAsync(string email, string password, string firstName, string lastName, Guid? clientId);
    Task<(bool Success, Guid UserId, string Email, Guid? ClientId)> ValidateCredentialsAsync(string email, string password);
    Task<IList<string>> GetUserRolesAsync(Guid userId);
    Task<(string Email, Guid? ClientId)?> GetUserInfoAsync(Guid userId);
}
