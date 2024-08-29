using Domain.Entities;
using Shared.Utils;

namespace Domain.Services.Contracts
{
    public interface ICompleteRentService
    {
        Task<Either<Error, RentEntity>> Complete(Guid rentId, CancellationToken cancellationToken);
    }
}
