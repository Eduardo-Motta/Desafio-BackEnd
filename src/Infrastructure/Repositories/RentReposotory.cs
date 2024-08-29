using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class RentReposotory : IRentReposotory
    {
        private readonly DatabaseContext _context;

        public RentReposotory(DatabaseContext context)
        {
            _context = context;
        }

        public async Task CreateRent(RentEntity entity, CancellationToken cancellationToken)
        {
            await _context.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> ExistsRentToMotorcycleId(Guid motorcycleId, CancellationToken cancellationToken)
        {
            return await _context.Rents.AnyAsync(x => x.MotorcycleId == motorcycleId);
        }

        public async Task<bool> ExistsRentInProgressToMotorcycleId(Guid motorcycleId, CancellationToken cancellationToken)
        {
            return await _context.Rents.AnyAsync(x => x.MotorcycleId == motorcycleId && x.Status == Domain.Enums.ERentStatus.InProgress);
        }

        public async Task<bool> ExistsRentInProgressToCourierId(Guid courierId, CancellationToken cancellationToken)
        {
            return await _context.Rents.AnyAsync(x => x.CourierId == courierId && x.Status == Domain.Enums.ERentStatus.InProgress);
        }

        public async Task<RentEntity?> FindRentById(Guid rentId, CancellationToken cancellationToken)
        {
            return await _context.Rents
                .Include(x => x.RentalClosure)
                .Include(x => x.Plan)
                .Where(x => x.Id == rentId)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task UpdateRent(RentEntity entity, CancellationToken cancellationToken)
        {
            _context.Rents.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
