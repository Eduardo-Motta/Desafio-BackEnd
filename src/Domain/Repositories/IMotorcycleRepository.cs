using Domain.Entities;

namespace Domain.Repositories
{
    public interface IMotorcycleRepository
    {
        Task<MotorcycleEntity?> FindMotorcycleByLicensePlate(string licensePlate, CancellationToken cancellationToken);
        Task<IEnumerable<MotorcycleEntity>> AllMotorcycles(CancellationToken cancellationToken);
        Task<MotorcycleEntity?> FindMotorcycleById(Guid id, CancellationToken cancellationToken);
        Task CreateMotorcycle(MotorcycleEntity entity, CancellationToken cancellationToken);
        Task UpdateMotorcycle(MotorcycleEntity entity, CancellationToken cancellationToken);
        Task DeleteMotorcycle(MotorcycleEntity entity, CancellationToken cancellationToken);
        Task<IEnumerable<MotorcycleEntity>> AllAvailableMotorcycles(CancellationToken cancellationToken);
    }
}
