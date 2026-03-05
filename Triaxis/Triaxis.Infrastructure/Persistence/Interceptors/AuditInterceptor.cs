using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Triaxis.Application.Common.Interfaces;
using Triaxis.Domain.Entities;

namespace Triaxis.Infrastructure.Persistence.Interceptors;

public class AuditInterceptor : SaveChangesInterceptor
{
    private readonly ICurrentUser _currentUser;

    public AuditInterceptor(ICurrentUser currentUser)
    {
        _currentUser = currentUser;
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        var context = eventData.Context;
        if (context is null)
            return await base.SavingChangesAsync(eventData, result, cancellationToken);

        var auditEntries = new List<AuditLog>();
        var now = DateTime.UtcNow;

        foreach (var entry in context.ChangeTracker.Entries())
        {
            if (entry.Entity is AuditLog || entry.State is EntityState.Detached or EntityState.Unchanged)
                continue;

            var entityType = entry.Entity.GetType().Name;
            var entityId = entry.Properties.FirstOrDefault(p => p.Metadata.Name == "Id")?.CurrentValue?.ToString() ?? "";

            var auditLog = new AuditLog
            {
                EntityType = entityType,
                EntityId = entityId,
                Action = entry.State.ToString(),
                UserId = _currentUser.UserId != Guid.Empty ? _currentUser.UserId : null,
                Timestamp = now,
                IpAddress = _currentUser.IpAddress
            };

            switch (entry.State)
            {
                case EntityState.Added:
                    var addedValues = entry.Properties
                        .Where(p => p.CurrentValue is not null)
                        .ToDictionary(p => p.Metadata.Name, p => (object?)p.CurrentValue);
                    auditLog.Changes = JsonSerializer.Serialize(addedValues);
                    break;

                case EntityState.Modified:
                    var changes = new Dictionary<string, object?>();
                    foreach (var property in entry.Properties.Where(p => p.IsModified))
                    {
                        changes[property.Metadata.Name] = new
                        {
                            Old = property.OriginalValue,
                            New = property.CurrentValue
                        };
                    }
                    auditLog.Changes = JsonSerializer.Serialize(changes);
                    break;

                case EntityState.Deleted:
                    var deletedValues = entry.Properties
                        .ToDictionary(p => p.Metadata.Name, p => (object?)p.OriginalValue);
                    auditLog.Changes = JsonSerializer.Serialize(deletedValues);
                    break;
            }

            auditEntries.Add(auditLog);
        }

        if (auditEntries.Count > 0)
            context.Set<AuditLog>().AddRange(auditEntries);

        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
