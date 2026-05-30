using Catalog.Domain.Establishments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Infrastructure.Persistence.Establishments;

public class EstablishmentConfiguration : IEntityTypeConfiguration<Establishment>
{
    public void Configure(EntityTypeBuilder<Establishment> builder)
    {
        builder.ToTable("Establishments");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedNever();

        builder.Property(e => e.CommercialName).HasMaxLength(100).IsRequired();
        builder.Property(e => e.Address).HasMaxLength(200).IsRequired();
        builder.Property(e => e.PhoneNumber).HasMaxLength(20).IsRequired();

        builder.OwnsMany(e => e.WeeklySchedules, wsBuilder =>
        {
            wsBuilder.WithOwner().HasForeignKey("EstablishmentId");

            wsBuilder.Property<Guid>("Id").ValueGeneratedOnAdd();
            wsBuilder.HasKey("Id");

            wsBuilder.Property(w => w.DayOfWeek)
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();

            wsBuilder.Property(w => w.OpeningTime)
                .HasPrecision(0)
                .IsRequired();

            wsBuilder.Property(w => w.ClosingTime)
                .HasPrecision(0)
                .IsRequired();
        });
    }
}