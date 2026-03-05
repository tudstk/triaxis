using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Triaxis.Domain.Entities;

namespace Triaxis.Infrastructure.Persistence.Configurations;

public class StudyConfiguration : IEntityTypeConfiguration<Study>
{
    public void Configure(EntityTypeBuilder<Study> builder)
    {
        builder.ToTable("Studies");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Name)
            .HasMaxLength(300)
            .IsRequired();

        builder.Property(s => s.ProtocolNumber)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(s => s.Phase)
            .HasMaxLength(50);

        builder.Property(s => s.Status)
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.HasIndex(s => new { s.ClientId, s.ProtocolNumber })
            .IsUnique();

        builder.OwnsOne(s => s.Settings, sb =>
        {
            sb.ToJson();
        });
    }
}
