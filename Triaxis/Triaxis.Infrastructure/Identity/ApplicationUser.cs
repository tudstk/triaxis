using Microsoft.AspNetCore.Identity;

namespace Triaxis.Infrastructure.Identity;

public class ApplicationUser : IdentityUser<Guid>
{
    public Guid? ClientId { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
}
