using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.Configurations
{
    [ExcludeFromCodeCoverage]
    public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.ToTable("User");

            builder.HasKey(b => b.Id);

            builder.Property(b => b.Id)
            .HasColumnType("uuid");

            builder.Property(b => b.Identity)
                .IsRequired();

            builder.Property(b => b.Password)
                .IsRequired();

            builder.Property(b => b.Role)
                .HasConversion(
                       x => x.ToString(),
                       x => (EUserRole)Enum.Parse(typeof(EUserRole), x)
                   )
                   .HasColumnType("VARCHAR(20)");

            builder.Property(b => b.CreatedAt)
                .IsRequired();

            builder.Property(b => b.UpdatedAt)
                .IsRequired(false);
        }
    }
}
