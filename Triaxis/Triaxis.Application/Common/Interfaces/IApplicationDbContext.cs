using Microsoft.EntityFrameworkCore;
using Triaxis.Domain.Entities;

namespace Triaxis.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<AuditLog> AuditLogs { get; }
    DbSet<Client> Clients { get; }
    DbSet<Study> Studies { get; }
    DbSet<RefreshToken> RefreshTokens { get; }
    DbSet<VisitDefinition> VisitDefinitions { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
