using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.Configurations
{
    [ExcludeFromCodeCoverage]
    public class MotorcycleConfiguration : IEntityTypeConfiguration<MotorcycleEntity>
    {
        public void Configure(EntityTypeBuilder<MotorcycleEntity> builder)
        {
            builder.ToTable("Motorcycle");

            builder.HasKey(b => b.Id);

            builder.Property(b => b.Id)
            .HasColumnType("uuid");

            builder.Property(b => b.Model)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(b => b.LicensePlate)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(b => b.CreatedAt)
                .IsRequired();

            builder.Property(b => b.UpdatedAt)
                .IsRequired(false);
        }
    }
}
