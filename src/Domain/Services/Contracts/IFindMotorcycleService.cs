using Domain.Entities;
using Shared.Utils;

namespace Domain.Services.Contracts
{
    public interface IFindMotorcycleService
    {
        Task<Either<Error, IEnumerable<MotorcycleEntity>>> All(CancellationToken cancellationToken);
        Task<Either<Error, MotorcycleEntity>> FindByLicensePlate(string licensePlate, CancellationToken cancellationToken);
        Task<Either<Error, IEnumerable<MotorcycleEntity>>> AllAvailableMotorcycles(CancellationToken cancellationToken);
    }
}
