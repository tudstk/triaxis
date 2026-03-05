using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Triaxis.Application.Common.Interfaces;
using Triaxis.Domain.Common;

namespace Triaxis.Infrastructure.Persistence.Interceptors;

public class SoftDeleteInterceptor : SaveChangesInterceptor
{
    private readonly ICurrentUser _currentUser;

    public SoftDeleteInterceptor(ICurrentUser currentUser)
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

        foreach (var entry in context.ChangeTracker.Entries<ISoftDelete>())
        {
            if (entry.State != EntityState.Deleted)
                continue;

            entry.State = EntityState.Modified;
            entry.Entity.DeletedAt = DateTime.UtcNow;
            entry.Entity.DeletedByUserId = _currentUser.UserId != Guid.Empty ? _currentUser.UserId : null;
        }

        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
