using Application.Commands.RentOut;
using Domain.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentApi.Extensions;
using Shared.Handlers;

namespace RentApi.Controllers
{
    public class RentController : BaseController
    {
        public RentController(ILogger<RentController> logger)
        {
        }

        /// <summary>
        /// Realiza a adesão de um aluguel
        /// </summary>
        /// 
        /// <remarks>planId: Consultar um plano no endpoint api/Plan.</remarks>
        /// <remarks>motorcycleId: Consultar uma moto disponível para locação no endpoint api/Motorcycle/AvailablesToRent.</remarks>
        /// <remarks>Este método requer a role 'Courier'.</remarks>
        /// 
        /// <response code="200">Retorna dados do aluguel e valor totalizado.</response>
        /// <response code="400">Plan not found.</response>
        /// <response code="400">Courier not found.</response>
        /// <response code="400">Only courier with category A can perform this operation.</response>
        /// <response code="400">Motorcycle not found.</response>
        /// <response code="400">The motorcycle provided is already rented.</response>
        /// <response code="400">An error occurred while creating the rental.</response>
        /// <response code="403">Se o usuário não tiver a role 'Courier'.</response>
        [Authorize(Roles = "Courier")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Create([FromServices] IHandler<CreateRentOutCommand> handler, [FromBody] CreateRentOutCommand command, CancellationToken cancellationToken)
        {
            command.SetCourierCnpj(User.GetUserIdentity());

            var result = await handler.Handle(command, cancellationToken);

            return HandleResponse(result);
        }

        /// <summary>
        /// Realiza simulação de valores para a adesão de um aluguel
        /// </summary>
        /// 
        /// <remarks>planId: Consultar um plano no endpoint api/Plan.</remarks>
        /// <remarks>motorcycleId: Consultar uma moto disponível para locação no endpoint api/Motorcycle/AvailablesToRent.</remarks>
        /// <remarks>Este método requer a role 'Courier'.</remarks>
        /// 
        /// <response code="200">Retorna detalhamento dos valores do aluguel.</response>
        /// <response code="400">Plan not found.</response>
        /// <response code="400">Courier not found.</response>
        /// <response code="400">Only courier with category A can perform this operation.</response>
        /// <response code="400">Motorcycle not found.</response>
        /// <response code="400">An error occurred while simulating the rental.</response>
        /// <response code="403">Se o usuário não tiver a role 'Courier'.</response>

        [Authorize(Roles = "Courier")]
        [HttpPost("Simulate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Simulate([FromServices] IHandler<SimulateRentOutCommand> handler, [FromBody] SimulateRentOutCommand command, CancellationToken cancellationToken)
        {
            command.SetCourierCnpj(User.GetUserIdentity());

            var result = await handler.Handle(command, cancellationToken);

            return HandleResponse(result);
        }

        /// <summary>
        /// Realiza conclusão de um aluguel em andamento
        /// </summary>
        /// 
        /// <remarks>Este método requer a role 'Courier'.</remarks>
        /// 
        /// <response code="200">Retorna dados resumidos da aluguel.</response>
        /// <response code="400">The rental has already been completed.</response>
        /// <response code="400">An error occurred while complete the rental.</response>
        /// <response code="403">Se o usuário não tiver a role 'Courier'.</response>
        /// <response code="404">Se nenhum aluguel for encontrado.</response>
        [Authorize(Roles = "Courier")]
        [HttpPost("{id}/Complete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Complete([FromServices] ICompleteRentService service, [FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var result = await service.Complete(id, cancellationToken);

            if (result.IsLeft)
            {
                return BadRequest(result.Left);
            }

            return Ok(result.Right);
        }
    }
}
