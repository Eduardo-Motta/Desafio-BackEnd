using Shared.Utils;

namespace Domain.Services.Contracts
{
    public interface IDeleteMotorcycleService
    {
        Task<Either<Error, bool>> Delete(Guid motorcycleId, CancellationToken cancellationToken);
    }
}
