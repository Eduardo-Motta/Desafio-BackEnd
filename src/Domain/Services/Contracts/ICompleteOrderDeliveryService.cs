using Domain.Entities;
using Shared.Utils;

namespace Domain.Services.Contracts
{
    public interface ICompleteOrderDeliveryService
    {
        Task<Either<Error, OrderEntity>> Complete(Guid orderId, Guid courierId, CancellationToken cancellationToken);
    }
}
