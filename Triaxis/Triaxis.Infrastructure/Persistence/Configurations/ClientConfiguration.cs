using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Triaxis.Domain.Entities;

namespace Triaxis.Infrastructure.Persistence.Configurations;

public class ClientConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.ToTable("Clients");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(c => c.Code)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(c => c.Code)
            .IsUnique();

        builder.Property(c => c.IsActive)
            .HasDefaultValue(true);

        builder.HasMany(c => c.Studies)
            .WithOne(s => s.Client)
            .HasForeignKey(s => s.ClientId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
