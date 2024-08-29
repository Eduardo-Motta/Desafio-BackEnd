using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.Context
{
    [ExcludeFromCodeCoverage]
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DatabaseContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<UserEntity> Users => Set<UserEntity>();
        public DbSet<MotorcycleEntity> Motorcycles => Set<MotorcycleEntity>();
        public DbSet<CourierEntity> Couriers => Set<CourierEntity>();
        public DbSet<PlanEntity> Plans => Set<PlanEntity>();
        public DbSet<RentEntity> Rents => Set<RentEntity>();
        public DbSet<RentalClosureEntity> RentalClosures => Set<RentalClosureEntity>();
        public DbSet<OrderEntity> Orders => Set<OrderEntity>();
        public DbSet<DeliveryAssignmentEntity> DeliveryAssignments => Set<DeliveryAssignmentEntity>();
        public DbSet<MotorcycleNotificationEntity> MotorcycleNotifications => Set<MotorcycleNotificationEntity>();
    }
}
