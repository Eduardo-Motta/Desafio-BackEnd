using Domain.Entities;

namespace Domain.Repositories
{
    public interface IRentReposotory
    {
        Task CreateRent(RentEntity entity, CancellationToken cancellationToken);
        Task UpdateRent(RentEntity entity, CancellationToken cancellationToken);
        Task<RentEntity?> FindRentById(Guid rentId, CancellationToken cancellationToken);
        Task<bool> ExistsRentToMotorcycleId(Guid motorcycleId, CancellationToken cancellationToken);
        Task<bool> ExistsRentInProgressToMotorcycleId(Guid motorcycleId, CancellationToken cancellationToken);
        Task<bool> ExistsRentInProgressToCourierId(Guid courierId, CancellationToken cancellationToken);
    }
}
