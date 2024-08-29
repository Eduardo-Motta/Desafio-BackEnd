using Shared.Utils;

namespace Domain.Services.Contracts
{
    public interface IUpdateMotorcycleService
    {
        Task<Either<Error, bool>> Update(Guid motorcycleId, string licensePlate, CancellationToken cancellationToken);
    }
}
