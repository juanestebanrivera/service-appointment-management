using Appointments.Domain.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Appointments.Infrastructure.Persistence.Services;

internal sealed class ServerConfiguration : IEntityTypeConfiguration<Service>
{
    public void Configure(EntityTypeBuilder<Service> builder)
    {
        builder.ToTable("Services");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).ValueGeneratedNever();

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(c => c.Price)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(c => c.Duration)
            .HasPrecision(0)
            .IsRequired();

        builder.Property(c => c.Description)
            .HasMaxLength(1000)
            .IsRequired(false);

        builder.Property(c => c.IsActive)
            .HasDefaultValue(true)
            .IsRequired();
    }
}