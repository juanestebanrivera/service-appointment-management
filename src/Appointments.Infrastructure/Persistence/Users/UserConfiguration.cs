using Appointments.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Appointments.Infrastructure.Persistence.Users;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).ValueGeneratedNever();

        builder.OwnsOne(c => c.Email, emailBuilder =>
        {
            emailBuilder.Property(e => e.Value)
                        .HasColumnName("Email")
                        .HasMaxLength(255)
                        .IsRequired();

            emailBuilder.HasIndex(e => e.Value)
                        .IsUnique()
                        .HasDatabaseName("IX_User_Email_Unique");
        });

        builder.Property(c => c.PasswordHash)
               .HasMaxLength(255)
               .IsRequired();

        builder.Property(c => c.Role)
               .HasConversion<string>()
               .HasMaxLength(20)
               .IsRequired();

        builder.Property(c => c.IsActive)
               .HasDefaultValue(true)
               .IsRequired();
    }
}