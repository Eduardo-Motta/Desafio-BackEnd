using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.Configurations
{

    [ExcludeFromCodeCoverage]
    public class OrderConfiguration : IEntityTypeConfiguration<OrderEntity>
    {
        public void Configure(EntityTypeBuilder<OrderEntity> builder)
        {
            builder.ToTable("Order");

            builder.HasKey(b => b.Id);

            builder.Property(b => b.Id)
            .HasColumnType("uuid");

            builder.Property(b => b.DeliveryStatus)
                .HasConversion(
                       x => x.ToString(),
                       x => (EOrderDeliveryStatus)Enum.Parse(typeof(EOrderDeliveryStatus), x)
                   )
                   .HasColumnType("VARCHAR(20)");

            builder.Property(b => b.CreatedAt)
                .IsRequired();

            builder.Property(b => b.UpdatedAt)
                .IsRequired(false);

            builder.HasOne(x => x.DeliveryAssignment)
            .WithOne(x => x.Order)
            .HasForeignKey<DeliveryAssignmentEntity>(x => x.OrderId);
        }
    }
}
