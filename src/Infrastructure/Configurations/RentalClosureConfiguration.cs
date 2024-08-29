using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.Configurations
{
    [ExcludeFromCodeCoverage]
    public class RentalClosureConfiguration : IEntityTypeConfiguration<RentalClosureEntity>
    {
        public void Configure(EntityTypeBuilder<RentalClosureEntity> builder)
        {
            builder.ToTable("RentalClosure");

            builder.HasKey(b => b.Id);

            builder.Property(b => b.Id)
            .HasColumnType("uuid");

            builder.Property(b => b.RentId)
                .IsRequired();

            builder.Property(b => b.EndDate)
                .IsRequired();

            builder.Property(b => b.ExceededDays)
                .IsRequired();

            builder.Property(b => b.PenaltyAmountForUnusedDay)
                .IsRequired();

            builder.Property(b => b.CostForUsedDays)
                .IsRequired();

            builder.Property(b => b.TotalAdditionalDailyAmount)
                .IsRequired();

            builder.Property(b => b.TotalPayment)
                .IsRequired();

            builder.Property(b => b.CreatedAt)
                .IsRequired();

            builder.Property(b => b.UpdatedAt)
                .IsRequired(false);

            builder.HasOne(rc => rc.Rent)
            .WithOne(r => r.RentalClosure)
            .HasForeignKey<RentalClosureEntity>(rc => rc.RentId)
            .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
