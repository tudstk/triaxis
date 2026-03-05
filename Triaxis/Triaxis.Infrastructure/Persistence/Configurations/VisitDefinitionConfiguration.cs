using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Triaxis.Domain.Entities;

namespace Triaxis.Infrastructure.Persistence.Configurations;

public class VisitDefinitionConfiguration : IEntityTypeConfiguration<VisitDefinition>
{
    public void Configure(EntityTypeBuilder<VisitDefinition> builder)
    {
        builder.ToTable("VisitDefinitions");

        builder.HasKey(v => v.Id);

        builder.Property(v => v.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(v => v.Code)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(v => v.VisitType)
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.HasIndex(v => new { v.StudyId, v.Code })
            .IsUnique()
            .HasFilter("\"StudyId\" IS NOT NULL");

        builder.HasIndex(v => v.IsDefault);
    }
}
