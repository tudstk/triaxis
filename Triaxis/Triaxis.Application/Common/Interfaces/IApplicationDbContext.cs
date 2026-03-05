using Microsoft.EntityFrameworkCore;
using Triaxis.Domain.Entities;

namespace Triaxis.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<AuditLog> AuditLogs { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
