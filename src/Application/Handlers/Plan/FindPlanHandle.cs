using Application.Commands;
using Application.Commands.Plan;
using Domain.Entities;
using Domain.Services.Contracts;
using Microsoft.Extensions.Logging;
using Shared.Commands;
using Shared.Handlers;

namespace Application.Handlers.Plan
{
    public class FindPlanHandle : IHandler<FindAllPlansCommand>
    {
        private readonly ILogger _logger;
        private readonly IFindPlanService _findPlanService;

        public FindPlanHandle(IFindPlanService findPlanService, ILogger<FindPlanHandle> logger)
        {
            _findPlanService = findPlanService;
            _logger = logger;
        }

        public async Task<ICommandResult> Handle(FindAllPlansCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting search for all motorcycles");

            var result = await _findPlanService.All(cancellationToken);

            if (result.IsLeft)
            {
                return new CommandResponseError(result.Left.Message);
            }

            return new CommandResponseData<IEnumerable<PlanEntity>>(result.Right);
        }
    }
}
