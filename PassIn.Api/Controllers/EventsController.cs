using Microsoft.AspNetCore.Mvc;
using PassIn.Application.UseCases.Events.GetById;
using PassIn.Application.UseCases.Events.Register;
using PassIn.Communication.Requests;
using PassIn.Communication.Responses;

namespace PassIn.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        [HttpPost]
        //Setting up what will happen when there is a success and when there isn't
        //500 will not be here for it shall be given more care, since it is usually a bigger problem
        [ProducesResponseType(typeof(ResponseRegisteredJson), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        public IActionResult Register([FromBody] RequestEventJson request)
        {

            var useCase = new RegisterEventsUseCase();

            var response = useCase.Execute(request);

            return Created(string.Empty, response);
        }


        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(ResponseEventJson), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        //not found because the possible error is the Id not being found, as we are calling it
        public IActionResult GetById([FromRoute] Guid id)
        {
            var useCase = new GetEventByIdUseCase();

            var response = useCase.Execute(id);

            return Ok(response);
        }
    }
}

