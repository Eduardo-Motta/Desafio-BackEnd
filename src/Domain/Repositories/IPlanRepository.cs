using Domain.Entities;

namespace Domain.Repositories
{
    public interface IPlanRepository
    {
        Task<PlanEntity?> FindPlanById(Guid planId, CancellationToken cancellationToken);
        Task<IEnumerable<PlanEntity>> AllPlans(CancellationToken cancellationToken);
    }
}
