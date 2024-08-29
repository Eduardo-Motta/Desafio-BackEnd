using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.Configurations
{
    [ExcludeFromCodeCoverage]
    public class MotorcycleNotificationConfiguration : IEntityTypeConfiguration<MotorcycleNotificationEntity>
    {
        public void Configure(EntityTypeBuilder<MotorcycleNotificationEntity> builder)
        {
            builder.ToTable("MotorcycleNotification");

            builder.HasKey(b => b.Id);

            builder.Property(b => b.Id)
            .HasColumnType("uuid");

            builder.Property(b => b.Model)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(b => b.LicensePlate)
                .IsRequired()
                .HasMaxLength(20);
        }
    }
}
