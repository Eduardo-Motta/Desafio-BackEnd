using Domain.Repositories;
using Domain.Services.Contracts;
using Microsoft.Extensions.Logging;
using Shared.Utils;

namespace Domain.Services.Order
{
    public class OrderDeliveryAssignmentService : IOrderDeliveryAssignmentService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICourierRespository _courierRespository;
        private readonly IRentReposotory _rentOutReposotory;
        private readonly ILogger _logger;

        public OrderDeliveryAssignmentService(IOrderRepository orderRepository, IRentReposotory rentOutReposotory, ICourierRespository courierRespository, ILogger<OrderDeliveryAssignmentService> logger)
        {
            _orderRepository = orderRepository;
            _rentOutReposotory = rentOutReposotory;
            _courierRespository = courierRespository;
            _logger = logger;
        }

        public async Task<Either<Error, bool>> DeliveryAssignment(Guid orderId, Guid courierId, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Courier with id {CourierId} assigning order with id {OrderId} for delivery", courierId, orderId);

                var order = await _orderRepository.FindOrderById(orderId, cancellationToken);

                if (order is null)
                {
                    _logger.LogWarning("Order with this id not found: {OrderId}", orderId);
                    return Either<Error, bool>.LeftValue(new Error("Not found"));
                }

                if (order.DeliveryStatus != Enums.EOrderDeliveryStatus.Pending)
                {
                    _logger.LogWarning("Order is no longer available for delivery: {@Order}", order);
                    return Either<Error, bool>.LeftValue(new Error("Order not available"));
                }

                var courier = await _courierRespository.FindCourierById(courierId, cancellationToken);

                if (courier is null)
                {
                    _logger.LogWarning("Courier with this id not found: {CourierId}", courierId);
                    return Either<Error, bool>.LeftValue(new Error("Courier not found"));
                }

                var courierHasRentInProgress = await _rentOutReposotory.ExistsRentInProgressToCourierId(courierId, cancellationToken);

                if (!courierHasRentInProgress)
                {
                    _logger.LogWarning("Courier with id{CourierId} does not have an active rental", courierId);
                    return Either<Error, bool>.LeftValue(new Error("Only couriers with an active rental can make deliveries"));
                }

                order.StartDelivery(courierId);

                await _orderRepository.UpdateOrder(order, cancellationToken);

                _logger.LogInformation("Order assigned to delivery: {@Order}", order);

                return Either<Error, bool>.RightValue(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error while assignment the order for delivery");
                return Either<Error, bool>.LeftValue(new Error("An error occurred while assignment the order for delivery"));
            }
        }
    }
}
