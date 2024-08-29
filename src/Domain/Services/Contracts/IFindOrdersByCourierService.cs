using Domain.Entities;
using Shared.Utils;

namespace Domain.Services.Contracts
{
    public interface IFindOrdersByCourierService
    {
        Task<Either<Error, IEnumerable<OrderEntity>>> All(Guid courierId, CancellationToken cancellationToken);
    }
}
