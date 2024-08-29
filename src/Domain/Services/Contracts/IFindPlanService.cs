using Domain.Entities;
using Shared.Utils;

namespace Domain.Services.Contracts
{
    public interface IFindPlanService
    {
        Task<Either<Error, IEnumerable<PlanEntity>>> All(CancellationToken cancellationToken);
    }
}
