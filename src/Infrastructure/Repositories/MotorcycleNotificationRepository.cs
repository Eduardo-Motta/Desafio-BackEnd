using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Context;

namespace Infrastructure.Repositories
{
    public class MotorcycleNotificationRepository : IMotorcycleNotificationRepository
    {
        private readonly DatabaseContext _context;

        public MotorcycleNotificationRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task CreateMotorcycleNotification(MotorcycleNotificationEntity entity, CancellationToken cancellationToken)
        {
            await _context.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
