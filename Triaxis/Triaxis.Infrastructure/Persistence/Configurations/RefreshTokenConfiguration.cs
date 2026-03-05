using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Triaxis.Domain.Entities;

namespace Triaxis.Infrastructure.Persistence.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("RefreshTokens");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Token)
            .HasMaxLength(500)
            .IsRequired();

        builder.HasIndex(t => t.Token)
            .IsUnique();

        builder.HasIndex(t => t.UserId);

        builder.Ignore(t => t.IsExpired);
        builder.Ignore(t => t.IsRevoked);
        builder.Ignore(t => t.IsActive);
    }
}
