using Domain.Entities;
using Domain.Repositories;
using Domain.Services.Contracts;
using Microsoft.Extensions.Logging;
using Shared.Utils;

namespace Domain.Services.Order
{
    public class FindOrdersByCourierService : IFindOrdersByCourierService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger _logger;

        public FindOrdersByCourierService(IOrderRepository orderRepository, ILogger<FindOrdersByCourierService> logger)
        {
            _orderRepository = orderRepository;
            _logger = logger;
        }

        public async Task<Either<Error, IEnumerable<OrderEntity>>> All(Guid courierId, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Start search for all orders by courier with id: {CourierId}", courierId);

                var orders = await _orderRepository.GetOrdersByCourier(courierId, cancellationToken);
                return Either<Error, IEnumerable<OrderEntity>>.RightValue(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while searching for all orders by courier");
                return Either<Error, IEnumerable<OrderEntity>>.LeftValue(new Error("An error occurred while searching for all orders by courier"));
            }
        }
    }
}
