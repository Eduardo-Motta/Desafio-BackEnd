using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class DeliveryAssignmentConfiguration : IEntityTypeConfiguration<DeliveryAssignmentEntity>
    {
        public void Configure(EntityTypeBuilder<DeliveryAssignmentEntity> builder)
        {
            builder.ToTable("DeliveryAssignment");

            builder.HasKey(b => b.Id);

            builder.Property(b => b.Id)
            .HasColumnType("uuid");

            builder.Property(b => b.CourierId)
               .IsRequired();

            builder.Property(b => b.OrderId)
                .IsRequired();

            builder.Property(b => b.AssignedAt)
                .IsRequired();

            builder.Property(b => b.CompletedAt)
                .IsRequired(false);

            builder.Property(b => b.CreatedAt)
                .IsRequired();

            builder.Property(b => b.UpdatedAt)
                .IsRequired(false);
        }
    }
}
