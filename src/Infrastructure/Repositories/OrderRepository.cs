using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DatabaseContext _context;

        public OrderRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OrderEntity>> AllOrders(CancellationToken cancellationToken)
        {
            return await _context.Orders.ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<OrderEntity>> AllAvailablesOrder(CancellationToken cancellationToken)
        {
            return await _context.Orders.Where(x => x.DeliveryStatus == Domain.Enums.EOrderDeliveryStatus.Pending).ToListAsync(cancellationToken);
        }

        public async Task CreateOrder(OrderEntity order, CancellationToken cancellationToken)
        {
            await _context.AddAsync(order, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<OrderEntity?> FindOrderById(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Orders
                .AsNoTracking()
                .Include(x => x.DeliveryAssignment)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IEnumerable<OrderEntity>> GetOrdersByCourier(Guid courierId, CancellationToken cancellationToken)
        {
            return await _context.Orders
                .Where(order => order.DeliveryAssignment != null && order.DeliveryAssignment.CourierId == courierId)
                .ToListAsync(cancellationToken);
        }

        public async Task UpdateOrder(OrderEntity order, CancellationToken cancellationToken)
        {
            if (order.DeliveryStatus != Domain.Enums.EOrderDeliveryStatus.Completed && order.DeliveryAssignment is not null)
            {
                _context.DeliveryAssignments.Add(order.DeliveryAssignment);
            }

            _context.Orders.Update(order);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
