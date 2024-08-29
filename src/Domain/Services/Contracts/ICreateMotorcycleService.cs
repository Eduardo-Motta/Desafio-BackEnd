using Domain.Entities;
using Shared.Utils;

namespace Domain.Services.Contracts
{
    public interface ICreateMotorcycleService
    {
        Task<Either<Error, Guid>> Create(MotorcycleEntity entity, CancellationToken cancellationToken);
    }
}
