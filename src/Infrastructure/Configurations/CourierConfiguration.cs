using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.Configurations
{
    [ExcludeFromCodeCoverage]
    public class CourierConfiguration : IEntityTypeConfiguration<CourierEntity>
    {
        public void Configure(EntityTypeBuilder<CourierEntity> builder)
        {
            builder.ToTable("Courier");

            builder.HasKey(b => b.Id);

            builder.Property(b => b.Id)
            .HasColumnType("uuid");

            builder.Property(b => b.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(b => b.Cnpj)
                .IsRequired()
                .HasMaxLength(14);

            builder.Property(b => b.BirthDate)
                .IsRequired()
                .HasColumnType("DATE");

            builder.Property(b => b.DrivingLicense)
                .IsRequired()
                .HasMaxLength(11);

            builder.Property(b => b.DrivingLicensePath)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(b => b.DrivingLicenseCategory)
                .HasConversion(
                       x => x.ToString(),
                       x => (EDrivingLicenseCategory)Enum.Parse(typeof(EDrivingLicenseCategory), x)
                   )
                   .HasColumnType("VARCHAR(2)");

            builder.Property(b => b.CreatedAt)
                .IsRequired();

            builder.Property(b => b.UpdatedAt)
                .IsRequired(false);
        }
    }
}
