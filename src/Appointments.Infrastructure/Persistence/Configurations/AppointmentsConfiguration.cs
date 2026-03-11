using Appointments.Domain.Entities;
using Appointments.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Appointments.Infrastructure.Persistence.Configurations;

class AppointmentsConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).ValueGeneratedNever();

        builder.HasOne(c => c.Client)
            .WithMany()
            .HasForeignKey(c => c.ClientId);

        builder.HasOne(c => c.Service)
            .WithMany()
            .HasForeignKey(c => c.ServiceId);

        builder.Property(c => c.ScheduledTime);
        builder.Property(c => c.Duration);

        builder.Property(c => c.Status)
            .HasDefaultValue(AppointmentStatus.Pending)
            .HasConversion<string>()
            .HasMaxLength(20);
    }
}