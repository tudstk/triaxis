using Microsoft.AspNetCore.Identity;

namespace Triaxis.Infrastructure.Identity;

public class ApplicationRole : IdentityRole<Guid>
{
    public RoleScope Scope { get; set; } = RoleScope.Global;
}

public enum RoleScope
{
    Global,
    Study
}
