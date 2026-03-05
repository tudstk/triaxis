using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Triaxis.Application.Common.Interfaces;

namespace Triaxis.Infrastructure.Identity;

public class CurrentUserService : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid UserId =>
        Guid.TryParse(_httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier), out var id)
            ? id
            : Guid.Empty;

    public Guid? ClientId =>
        Guid.TryParse(_httpContextAccessor.HttpContext?.User.FindFirstValue("ClientId"), out var id)
            ? id
            : null;

    public Guid? StudyId =>
        Guid.TryParse(_httpContextAccessor.HttpContext?.User.FindFirstValue("StudyId"), out var id)
            ? id
            : null;

    public string? IpAddress =>
        _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();

    public IReadOnlyList<string> Roles =>
        _httpContextAccessor.HttpContext?.User.FindAll(ClaimTypes.Role)
            .Select(c => c.Value)
            .ToList()
            .AsReadOnly()
        ?? (IReadOnlyList<string>)Array.Empty<string>();
}
