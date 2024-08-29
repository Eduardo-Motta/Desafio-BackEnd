using Domain.Entities;

namespace Domain.Repositories
{
    public interface IMotorcycleNotificationRepository
    {
        Task CreateMotorcycleNotification(MotorcycleNotificationEntity entity, CancellationToken cancellationToken);
    }
}
