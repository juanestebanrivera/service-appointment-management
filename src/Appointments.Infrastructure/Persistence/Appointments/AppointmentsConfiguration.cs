using Appointments.Domain.Appointments;
using Appointments.Domain.Clients;
using Appointments.Domain.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Appointments.Infrastructure.Persistence.Appointments;

internal sealed class AppointmentsConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.ToTable("Appointments");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).ValueGeneratedNever();

        builder.HasOne<Client>()
            .WithMany()
            .HasForeignKey(c => c.ClientId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Service>()
            .WithMany()
            .HasForeignKey(c => c.ServiceId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(c => c.PriceAtBooking)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(c => c.StartTime)
            .IsRequired();

        builder.Property(c => c.EndTime)
            .IsRequired();

        builder.Property(c => c.Status)
            .HasDefaultValue(AppointmentStatus.Pending)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();
    }
}