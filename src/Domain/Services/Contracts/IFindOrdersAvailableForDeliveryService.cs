using Domain.Entities;
using Shared.Utils;

namespace Domain.Services.Contracts
{
    public interface IFindOrdersAvailableForDeliveryService
    {
        Task<Either<Error, IEnumerable<OrderEntity>>> AvailableForDelivery(CancellationToken cancellationToken);
    }
}
