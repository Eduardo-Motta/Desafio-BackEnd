using Domain.Entities;
using Domain.Repositories;
using Domain.Services.Contracts;
using Microsoft.Extensions.Logging;
using Shared.Utils;

namespace Domain.Services.Plan
{
    public class FindPlanService : IFindPlanService
    {
        private readonly IPlanRepository _planRepository;
        private readonly ILogger _logger;

        public FindPlanService(IPlanRepository planRepository, ILogger<FindPlanService> logger)
        {
            _planRepository = planRepository;
            _logger = logger;
        }

        public async Task<Either<Error, IEnumerable<PlanEntity>>> All(CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Starting service to search for plans");

                var motorcycles = await _planRepository.AllPlans(cancellationToken);

                return Either<Error, IEnumerable<PlanEntity>>.RightValue(motorcycles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while searching for the plans");
                return Either<Error, IEnumerable<PlanEntity>>.LeftValue(new Error("An error occurred while searching for the plans"));
            }
        }
    }
}
