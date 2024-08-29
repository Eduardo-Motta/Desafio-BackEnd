using Application.Commands.Motorcycle;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Handlers;

namespace RentApi.Controllers
{
    public class MotorcycleController : BaseController
    {
        public MotorcycleController(ILogger<MotorcycleController> logger) { }

        /// <summary>
        /// Realiza a criação de uma moto.
        /// </summary>
        /// 
        /// <remarks>Este método requer a role 'Admin'.</remarks>
        /// 
        /// <response code="200">Retorna um Guid Id da moto criada.</response>
        /// <response code="400">License plate is already in use.</response>
        /// <response code="400">An error occurred while creating the motorcycle.</response>
        /// <response code="403">Se o usuário não tiver a role 'Admin'.</response>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Create([FromServices] IHandler<CreateMotorcycleCommand> handler, [FromBody] CreateMotorcycleCommand command, CancellationToken cancellationToken)
        {
            var result = await handler.Handle(command, cancellationToken);

            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        /// <summary>
        /// Realiza a atualização de uma moto.
        /// </summary>
        /// 
        /// <remarks>Este método requer a role 'Admin'.</remarks>
        /// 
        /// <response code="200">Solicitação bem-sucedida, mas sem dados retornados.</response>
        /// <response code="400">License plate is already in use.</response>
        /// <response code="400">An error occurred while updating the motorcycle.</response>
        /// <response code="403">Se o usuário não tiver a role 'Admin'.</response>
        /// <response code="404">Moto não encontrada.</response>
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromServices] IHandler<UpdateMotorcycleCommand> handler, [FromBody] UpdateMotorcycleCommand command, [FromRoute] Guid id, CancellationToken cancellationToken)
        {
            command.SetMotorcycleId(id);

            var result = await handler.Handle(command, cancellationToken);

            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        /// <summary>
        /// Retorna todas as motos cadastradas.
        /// </summary>
        /// 
        /// <remarks>Este método requer a role 'Admin'.</remarks>
        /// 
        /// <response code="200">Retorna lista de motos ou uma lista vazia.</response>
        /// <response code="400">An error occurred while searching for the motorcycles.</response>
        /// <response code="403">Se o usuário não tiver a role 'Admin'.</response>
        [Authorize(Roles = "Admin")]
        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> FindAll([FromServices] IHandler<FindAllMotorcyclesCommand> handler, CancellationToken cancellationToken)
        {
            var result = await handler.Handle(new FindAllMotorcyclesCommand(), cancellationToken);

            return HandleResponse(result);
        }

        /// <summary>
        /// Retorna todas as motos disponíveis para locação.
        /// </summary>
        /// 
        /// <remarks>Este método requer a role 'Admin' ou 'Courier'.</remarks>
        /// 
        /// <response code="200">Retorna lista com todas as motos disponíveis para locação ou uma lista vazia.</response>
        /// <response code="400">An error occurred while searching for the motorcycle.</response>
        /// <response code="403">Se o usuário não tiver a role 'Admin' ou 'Courier'.</response>
        /// <response code="404">Moto não encontrada.</response>
        [Authorize(Roles = "Admin,Courier")]
        [HttpGet("AvailablesToRent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> FindAllAvailable([FromServices] IHandler<FindAllAvailableMotorcyclesCommand> handler, CancellationToken cancellationToken)
        {
            var result = await handler.Handle(new FindAllAvailableMotorcyclesCommand(), cancellationToken);

            return HandleResponse(result);
        }

        /// <summary>
        /// Retorna uma moto pesquisada pela placa.
        /// </summary>
        /// 
        /// <remarks>Este método requer a role 'Admin'.</remarks>
        /// 
        /// <response code="200">Retorna os dados da moto.</response>
        /// <response code="400">Ocorreu um erro interno no servidor.</response>
        /// <response code="403">Se o usuário não tiver a role 'Admin'.</response>
        /// <response code="404">Moto não encontrada.</response>
        [Authorize(Roles = "Admin")]
        [HttpGet("{licensePlate}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> FindByLicensePlate([FromServices] IHandler<FindMotorcycleByLicensePlateCommand> handler, [FromRoute] string licensePlate, CancellationToken cancellationToken)
        {
            var command = new FindMotorcycleByLicensePlateCommand { LicensePlate = licensePlate };
            var result = await handler.Handle(command, cancellationToken);

            return HandleResponse(result);
        }

        /// <summary>
        /// Realiza a exclusão de uma moto por id.
        /// </summary>
        /// 
        /// <remarks>Este método requer a role 'Admin'.</remarks>
        /// 
        /// <response code="204">Solicitação bem-sucedida, mas sem dados retornados.</response>
        /// <response code="400">Ocorreu um erro interno no servidor.</response>
        /// <response code="403">Se o usuário não tiver a role 'Admin'.</response>
        /// <response code="404">Moto não encontrada.</response>
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromServices] IHandler<DeleteMotorcycleCommand> handler, [FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var command = new DeleteMotorcycleCommand { Id = id };
            var result = await handler.Handle(command, cancellationToken);

            return HandleDeleteResponse(result);
        }
    }
}
