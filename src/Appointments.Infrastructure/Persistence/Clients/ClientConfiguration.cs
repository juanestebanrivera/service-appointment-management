using Appointments.Domain.Clients;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Appointments.Infrastructure.Persistence.Clients;

internal sealed class ClientConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.ToTable("Clients");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).ValueGeneratedNever();

        builder.Property(c => c.FirstName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(c => c.LastName)
            .HasMaxLength(100)
            .IsRequired();

        builder.OwnsOne(c => c.Phone, phoneBuilder =>
        {
           phoneBuilder.Property(p => p.Prefix)
               .HasColumnName("PhonePrefix")
               .HasMaxLength(6)
               .IsRequired();

            phoneBuilder.Property(p => p.Number)
                .HasColumnName("PhoneNumber")
                .HasMaxLength(20)
                .IsRequired();

            phoneBuilder.HasIndex(p => new { p.Prefix, p.Number })
                .IsUnique()
                .HasDatabaseName("IX_Phone_Unique");
        });

        builder.OwnsOne(c => c.Email, emailBuilder =>
        {
            emailBuilder.Property(e => e.Value)
                .HasColumnName("Email")
                .HasMaxLength(255)
                .IsRequired();

            emailBuilder.HasIndex(e => e.Value)
                .IsUnique()
                .HasFilter("\"Email\" IS NOT NULL")
                .HasDatabaseName("IX_Email_Unique");
        });

        builder.Property(c => c.IsActive)
            .HasDefaultValue(true)
            .IsRequired();
    }
}