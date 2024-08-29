using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CourierRespository : ICourierRespository
    {
        private readonly DatabaseContext _context;

        public CourierRespository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task CreateCourier(CourierEntity entity, CancellationToken cancellationToken)
        {
            await _context.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<CourierEntity?> FindCourierByCnpj(string cnpj, CancellationToken cancellationToken)
        {
            return await _context.Couriers.Where(x => x.Cnpj == cnpj).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<CourierEntity?> FindCourierByDrivingLicense(string drivingLicense, CancellationToken cancellationToken)
        {
            return await _context.Couriers.Where(x => x.DrivingLicense == drivingLicense).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<CourierEntity?> FindCourierById(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Couriers.Where(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task UpdateCourier(CourierEntity entity, CancellationToken cancellationToken)
        {
            _context.Couriers.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
