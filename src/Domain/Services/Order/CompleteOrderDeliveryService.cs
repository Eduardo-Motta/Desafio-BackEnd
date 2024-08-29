using Domain.Entities;
using Domain.Repositories;
using Domain.Services.Contracts;
using Microsoft.Extensions.Logging;
using Shared.Utils;

namespace Domain.Services.Order
{
    public class CompleteOrderDeliveryService : ICompleteOrderDeliveryService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger _logger;

        public CompleteOrderDeliveryService(IOrderRepository orderRepository, IRentReposotory rentOutReposotory, ICourierRespository courierRespository, ILogger<CompleteOrderDeliveryService> logger)
        {
            _orderRepository = orderRepository;
            _logger = logger;
        }

        public async Task<Either<Error, OrderEntity>> Complete(Guid orderId, Guid courierId, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Completing the delivery assignment with order id: {OrderId}", orderId);

                var order = await _orderRepository.FindOrderById(orderId, cancellationToken);

                if (order is null)
                {
                    _logger.LogWarning("The order with this id not found: {OrderId}", orderId);
                    return Either<Error, OrderEntity>.LeftValue(new Error("Not found"));
                }

                if (order.DeliveryAssignment?.CourierId != courierId)
                {
                    _logger.LogWarning("The order with id {OrderId} was found but does not belong to the user with id {CourierId}", orderId, courierId);
                    return Either<Error, OrderEntity>.LeftValue(new Error("Not found"));
                }

                if (order.DeliveryStatus != Enums.EOrderDeliveryStatus.InProgress)
                {
                    _logger.LogWarning("The order status is invalid to complete. Only orders in progress can be completed. {@Order}", order);
                    return Either<Error, OrderEntity>.LeftValue(new Error("Invalid status. Only orders in progress can be completed."));
                }

                order.CompletedDelivery();

                await _orderRepository.UpdateOrder(order, cancellationToken);

                return Either<Error, OrderEntity>.RightValue(order);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error while complete the order");
                return Either<Error, OrderEntity>.LeftValue(new Error("An error occurred while complete the order"));
            }
        }
    }
}
