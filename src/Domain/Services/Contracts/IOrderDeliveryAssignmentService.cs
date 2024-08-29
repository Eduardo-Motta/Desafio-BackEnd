using Shared.Utils;

namespace Domain.Services.Contracts
{
    public interface IOrderDeliveryAssignmentService
    {
        Task<Either<Error, bool>> DeliveryAssignment(Guid orderId, Guid courierId, CancellationToken cancellationToken);
    }
}
