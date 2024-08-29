using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class MotorcycleRepository : IMotorcycleRepository
    {
        private readonly DatabaseContext _context;

        public MotorcycleRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MotorcycleEntity>> AllMotorcycles(CancellationToken cancellationToken)
        {
            return await _context.Motorcycles.ToListAsync(cancellationToken);
        }

        public async Task CreateMotorcycle(MotorcycleEntity entity, CancellationToken cancellationToken)
        {
            await _context.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteMotorcycle(MotorcycleEntity entity, CancellationToken cancellationToken)
        {
            _context.Motorcycles.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<MotorcycleEntity?> FindMotorcycleById(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Motorcycles.Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<MotorcycleEntity?> FindMotorcycleByLicensePlate(string licensePlate, CancellationToken cancellationToken)
        {
            return await _context.Motorcycles.Where(x => x.LicensePlate.Equals(licensePlate)).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task UpdateMotorcycle(MotorcycleEntity entity, CancellationToken cancellationToken)
        {
            _context.Motorcycles.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<MotorcycleEntity>> AllAvailableMotorcycles(CancellationToken cancellationToken)
        {
            return await _context.Motorcycles
                .Where(motorcycle => !_context.Rents
                    .Any(rent => rent.MotorcycleId == motorcycle.Id && rent.Status == ERentStatus.InProgress))
                .ToListAsync(cancellationToken);
        }
    }
}
