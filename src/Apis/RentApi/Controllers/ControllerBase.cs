using Application.Commands;
using Microsoft.AspNetCore.Mvc;
using Shared.Commands;

namespace RentApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController : ControllerBase
    {
        protected IActionResult HandleDeleteResponse(ICommandResult response)
        {
            if (response.Success)
            {
                return NoContent();
            }

            var responseError = (CommandResponseError)response;

            if (responseError.Message.Equals("Not found"))
                return NotFound();

            return BadRequest(response);
        }

        protected IActionResult HandleResponse(ICommandResult response)
        {
            if (response.Success)
            {
                return Ok(response);
            }

            if (response is CommandResponseErrors)
            {
                return BadRequest(response);
            }

            var responseError = (CommandResponseError)response;

            if (responseError.Message.Equals("Not found"))
                return NotFound();

            return BadRequest(response);
        }
    }
}
