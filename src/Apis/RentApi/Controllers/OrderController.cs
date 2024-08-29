using Domain.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentApi.Extensions;

namespace RentApi.Controllers
{
    public class OrderController : BaseController
    {
        public OrderController(ILogger<OrderController> logger) { }

        /// <summary>
        /// Retornar todos os pedidos disponíveis para serem entregues.
        /// </summary>
        /// 
        /// <remarks>Este método requer a role 'Admin' ou 'Courier'.</remarks>
        /// 
        /// <response code="200">Retornar os pedidos disponíveis para serem entregues.</response>
        /// <response code="400">An error occurred while searching for available orders to delivery.</response>
        /// <response code="403">Se o usuário não tiver a role 'Admin' ou 'Courier'.</response>
        [Authorize(Roles = "Courier,Admin")]
        [HttpGet("AvailableForDelivery")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> FindOrdersAvailableForDelivery([FromServices] IFindOrdersAvailableForDeliveryService service, CancellationToken cancellationToken)
        {
            var result = await service.AvailableForDelivery(cancellationToken);

            if (result.IsLeft)
            {
                return BadRequest(result.Left);
            }

            return Ok(result.Right);
        }

        /// <summary>
        /// Retornar todos os pedidos disponíveis para serem entregues.
        /// </summary>
        /// 
        /// <remarks>Este método requer a role 'Courier'.</remarks>
        /// 
        /// <response code="200">Retornar os pedidos concluídos e em progresso do entregador.</response>
        /// <response code="400">An error occurred while searching for all orders by courier.</response>
        /// <response code="403">Se o usuário não tiver a role 'Courier'.</response>
        [Authorize(Roles = "Courier")]
        [HttpGet("ByCourier")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> FindOrdersByCourier([FromServices] IFindOrdersByCourierService service, CancellationToken cancellationToken)
        {
            var result = await service.All(User.GetUserId(), cancellationToken);

            if (result.IsLeft)
            {
                return BadRequest(result.Left);
            }

            return Ok(result.Right);
        }

        /// <summary>
        /// Atribui a entrega de um pedido a um entregador.
        /// </summary>
        /// 
        /// <remarks>Este método requer a role 'Courier'.</remarks>
        /// 
        /// <response code="200">Solicitação bem-sucedida, mas sem dados retornados.</response>
        /// <response code="400">Order not available.</response>
        /// <response code="400">Courier not found.</response>
        /// <response code="400">Only couriers with an active rental can make deliveries.</response>
        /// <response code="400">An error occurred while assignment the order for delivery.</response>
        /// <response code="403">Se o usuário não tiver a role 'Courier'.</response>
        [Authorize(Roles = "Courier")]
        [HttpGet("{id}/DeliveryAssignment")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeliveryAssignment([FromServices] IOrderDeliveryAssignmentService service, [FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var result = await service.DeliveryAssignment(id, User.GetUserId(), cancellationToken);

            if (result.IsLeft)
            {
                return BadRequest(result.Left);
            }

            return Ok();
        }

        /// <summary>
        /// Conclui a entrega de um pedido em andamento.
        /// </summary>
        /// 
        /// <remarks>Este método requer a role 'Courier'.</remarks>
        /// 
        /// <response code="200">Solicitação bem-sucedida, mas sem dados retornados.</response>
        /// <response code="400">Invalid status. Only orders in progress can be completed.</response>
        /// <response code="400">An error occurred while complete the order.</response>
        /// <response code="403">Se o usuário não tiver a role 'Courier'.</response>
        [Authorize(Roles = "Courier")]
        [HttpGet("{id}/Complete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Complete([FromServices] ICompleteOrderDeliveryService service, [FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var result = await service.Complete(id, User.GetUserId(), cancellationToken);

            if (result.IsLeft)
            {
                return BadRequest(result.Left);
            }

            return Ok();
        }
    }
}
