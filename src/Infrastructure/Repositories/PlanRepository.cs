using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class PlanRepository : IPlanRepository
    {
        private readonly DatabaseContext _context;
        public PlanRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PlanEntity>> AllPlans(CancellationToken cancellationToken)
        {
            return await _context.Plans.ToListAsync(cancellationToken);
        }

        public async Task<PlanEntity?> FindPlanById(Guid planId, CancellationToken cancellationToken)
        {
            return await _context.Plans.Where(x => x.Id == planId).FirstOrDefaultAsync(cancellationToken);
        }
    }
}
