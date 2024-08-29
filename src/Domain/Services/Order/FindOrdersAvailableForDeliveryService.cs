using Domain.Entities;
using Domain.Repositories;
using Domain.Services.Contracts;
using Microsoft.Extensions.Logging;
using Shared.Utils;

namespace Domain.Services.Order
{
    public class FindOrdersAvailableForDeliveryService : IFindOrdersAvailableForDeliveryService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger _logger;

        public FindOrdersAvailableForDeliveryService(IOrderRepository orderRepository, ILogger<FindOrdersAvailableForDeliveryService> logger)
        {
            _orderRepository = orderRepository;
            _logger = logger;
        }

        public async Task<Either<Error, IEnumerable<OrderEntity>>> AvailableForDelivery(CancellationToken cancellationToken)
        {
            try
            {
                var orders = await _orderRepository.AllAvailablesOrder(cancellationToken);
                return Either<Error, IEnumerable<OrderEntity>>.RightValue(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while searching for all available orders to delivery");
                return Either<Error, IEnumerable<OrderEntity>>.LeftValue(new Error("An error occurred while searching for available orders to delivery"));
            }
        }
    }
}
