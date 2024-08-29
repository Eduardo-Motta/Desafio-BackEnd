using Application.Commands.Plan;
using Microsoft.AspNetCore.Mvc;
using Shared.Handlers;

namespace RentApi.Controllers
{
    public class PlanController : BaseController
    {
        public PlanController(ILogger<PlanController> logger) { }

        /// <summary>
        /// Retorna todos os planos de locação disponíveis.
        /// </summary>
        /// 
        /// <response code="200">Retorna lista com todos os planos de aluguel.</response>
        /// <response code="400">An error occurred while searching for the plans.</response>
        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> FindAll([FromServices] IHandler<FindAllPlansCommand> handler, CancellationToken cancellationToken)
        {
            var result = await handler.Handle(new FindAllPlansCommand(), cancellationToken);

            return HandleResponse(result);
        }
    }
}
