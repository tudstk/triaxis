using Microsoft.AspNetCore.Identity;
using Triaxis.Application.Common.Interfaces;
using Triaxis.Domain.Common;

namespace Triaxis.Infrastructure.Identity;

public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public IdentityService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result<Guid>> CreateUserAsync(string email, string password, string firstName, string lastName, Guid? clientId)
    {
        var user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            FirstName = firstName,
            LastName = lastName,
            ClientId = clientId
        };

        var result = await _userManager.CreateAsync(user, password);
        if (!result.Succeeded)
        {
            var errors = string.Join("; ", result.Errors.Select(e => e.Description));
            return Result<Guid>.Failure(errors);
        }

        return Result<Guid>.Success(user.Id);
    }

    public async Task<(bool Success, Guid UserId, string Email, Guid? ClientId)> ValidateCredentialsAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
            return (false, Guid.Empty, string.Empty, null);

        var valid = await _userManager.CheckPasswordAsync(user, password);
        if (!valid)
            return (false, Guid.Empty, string.Empty, null);

        return (true, user.Id, user.Email!, user.ClientId);
    }

    public async Task<IList<string>> GetUserRolesAsync(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null)
            return Array.Empty<string>();

        return await _userManager.GetRolesAsync(user);
    }

    public async Task<(string Email, Guid? ClientId)?> GetUserInfoAsync(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null)
            return null;

        return (user.Email!, user.ClientId);
    }
}
