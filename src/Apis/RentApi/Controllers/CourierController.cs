using Application.Commands.Courier;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentApi.Extensions;
using Shared.Handlers;

namespace RentApi.Controllers
{
    public class CourierController : BaseController
    {
        public CourierController(ILogger<CourierController> logger) { }

        /// <summary>
        /// Realiza cadastro do entregador
        /// </summary>
        /// <remarks>Após o cadastro, utilizar CNPJ e senha para realizar login.</remarks>
        /// <response code="200"></response>
        /// <response code="400">A courier with this CNPJ already exists: {Cnpj}.</response>
        /// <response code="400">Driving License is already in use.</response>
        /// <response code="400">An error occurred while creating the courier.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromServices] IHandler<CreateCourierCommand> handler, [FromBody] CreateCourierCommand command, CancellationToken cancellationToken)
        {
            var result = await handler.Handle(command, cancellationToken);

            return HandleResponse(result);
        }

        /// <summary>
        /// Realiza upload da imagem da CNH do entregador
        /// </summary>
        /// <remarks>Este método requer a role 'Courier'.</remarks>
        /// <response code="200">Retorna a URL da imagem.</response>
        /// <response code="400">An error occurred while uploading the driving license image.</response>
        /// <response code="403">Se o usuário não tiver a role 'Courier'.</response>
        /// <response code="404">Usuário não encontrado.</response>
        [Authorize(Roles = "Courier")]
        [HttpPost("UploadDrivingLicense")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> UploadDrivingLicense([FromServices] IHandler<UploadDrivingLicenseCommand> handler, [FromForm] UploadDrivingLicenseCommand command, CancellationToken cancellationToken)
        {
            command.SetCnpj(User.GetUserIdentity());

            var result = await handler.Handle(command, cancellationToken);

            return HandleResponse(result);
        }
    }
}
