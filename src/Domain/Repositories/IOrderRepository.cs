using Domain.Entities;

namespace Domain.Repositories
{
    public interface IOrderRepository
    {
        Task CreateOrder(OrderEntity order, CancellationToken cancellationToken);
        Task UpdateOrder(OrderEntity order, CancellationToken cancellationToken);
        Task<OrderEntity?> FindOrderById(Guid id, CancellationToken cancellationToken);
        Task<IEnumerable<OrderEntity>> AllOrders(CancellationToken cancellationToken);
        Task<IEnumerable<OrderEntity>> AllAvailablesOrder(CancellationToken cancellationToken);
        Task<IEnumerable<OrderEntity>> GetOrdersByCourier(Guid courierId, CancellationToken cancellationToken);
    }
}
