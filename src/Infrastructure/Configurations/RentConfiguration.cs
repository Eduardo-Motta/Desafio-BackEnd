using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.Configurations
{
    [ExcludeFromCodeCoverage]
    public class RentConfiguration : IEntityTypeConfiguration<RentEntity>
    {
        public void Configure(EntityTypeBuilder<RentEntity> builder)
        {
            builder.ToTable("Rent");

            builder.HasKey(b => b.Id);

            builder.Property(b => b.Id)
            .HasColumnType("uuid");

            builder.Property(b => b.CourierId)
                .IsRequired();

            builder.Property(b => b.PlanId)
                .IsRequired();

            builder.Property(b => b.MotorcycleId)
                .IsRequired();

            builder.Property(b => b.StartDate)
                .IsRequired();

            builder.Property(b => b.ExpectedEndDate)
                .IsRequired();

            builder.Property(b => b.EndDate)
                .IsRequired();

            builder.Property(b => b.Status)
                .HasConversion(
                       x => x.ToString(),
                       x => (ERentStatus)Enum.Parse(typeof(ERentStatus), x)
                   )
                   .HasColumnType("VARCHAR(50)");

            builder.Property(b => b.TotalPayment)
                .IsRequired();

            builder.Property(b => b.CreatedAt)
                .IsRequired();

            builder.Property(b => b.UpdatedAt)
                .IsRequired(false);

            builder.HasOne(r => r.RentalClosure)
            .WithOne(rc => rc.Rent)
            .HasForeignKey<RentalClosureEntity>(rc => rc.RentId)
            .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
