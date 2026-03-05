using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Triaxis.Application.Common.Interfaces;
using Triaxis.Domain.Common;
using Triaxis.Domain.Entities;
using Triaxis.Infrastructure.Identity;

namespace Triaxis.Infrastructure.Persistence;

public class TriaxisDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>, IApplicationDbContext
{
    private readonly ICurrentUser? _currentUser;

    public TriaxisDbContext(DbContextOptions<TriaxisDbContext> options) : base(options) { }

    public TriaxisDbContext(DbContextOptions<TriaxisDbContext> options, ICurrentUser currentUser) : base(options)
    {
        _currentUser = currentUser;
    }

    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Study> Studies => Set<Study>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<VisitDefinition> VisitDefinitions => Set<VisitDefinition>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(TriaxisDbContext).Assembly);

        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            if (typeof(ISoftDelete).IsAssignableFrom(entityType.ClrType))
            {
                var method = typeof(TriaxisDbContext)
                    .GetMethod(nameof(ApplySoftDeleteFilter), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)!
                    .MakeGenericMethod(entityType.ClrType);
                method.Invoke(null, [builder]);
            }

            if (typeof(IClientScoped).IsAssignableFrom(entityType.ClrType))
            {
                var method = typeof(TriaxisDbContext)
                    .GetMethod(nameof(ApplyClientScopeFilter), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
                    .MakeGenericMethod(entityType.ClrType);
                method.Invoke(this, [builder]);
            }
        }
    }

    private static void ApplySoftDeleteFilter<T>(ModelBuilder builder) where T : class, ISoftDelete
    {
        builder.Entity<T>().HasQueryFilter(e => e.DeletedAt == null);
    }

    private void ApplyClientScopeFilter<T>(ModelBuilder builder) where T : class, IClientScoped
    {
        builder.Entity<T>().HasQueryFilter(e => _currentUser == null || _currentUser.ClientId == null || e.ClientId == _currentUser.ClientId);
    }
}
