using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Triaxis.Domain.Entities;

namespace Triaxis.Infrastructure.Persistence.Configurations;

public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.ToTable("AuditLogs");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.EntityType).HasMaxLength(256).IsRequired();
        builder.Property(a => a.EntityId).HasMaxLength(256).IsRequired();
        builder.Property(a => a.Action).HasMaxLength(64).IsRequired();
        builder.Property(a => a.IpAddress).HasMaxLength(64);

        builder.HasIndex(a => a.Timestamp);
        builder.HasIndex(a => a.EntityType);
    }
}
