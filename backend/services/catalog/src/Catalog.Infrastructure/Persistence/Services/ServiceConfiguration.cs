using Catalog.Domain.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Infrastructure.Persistence.Services;

public class ServiceConfiguration : IEntityTypeConfiguration<Service>
{
    public void Configure(EntityTypeBuilder<Service> builder)
    {
        builder.ToTable("Services");

        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).ValueGeneratedNever();

        builder.Property(s => s.Name).HasMaxLength(200).IsRequired();
        builder.Property(s => s.Description).HasMaxLength(1000).IsRequired(false);

        builder.OwnsOne(s => s.Price, priceBuilder =>
        {
            priceBuilder.Property(p => p.Amount)
                        .HasColumnName("Price")
                        .HasPrecision(18, 2)
                        .IsRequired();

            priceBuilder.Property(p => p.Currency)
                        .HasMaxLength(3)
                        .IsRequired();
        });

        builder.OwnsOne(s => s.Duration, durationBuilder =>
            durationBuilder.Property(d => d.Minutes).IsRequired());

        builder.Property(s => s.IsActive).HasDefaultValue(true).IsRequired();
    }
}