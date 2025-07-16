using Eciton.Application.Abstractions;
using Eciton.Application.Commands.Auth;
using Eciton.Application.DTOs.Auth;
using Eciton.Application.ResponceObject.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Eciton.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost("login")]
        [SwaggerOperation(
        Summary = "Logs in the user.",
        Description = "Allows a user to log in with email/username and password. Returns a JWT token if successful."
        )]
        public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
        {
            var response = await _mediator.Send(command);

            if (response.ResponseStatusCode == ResponseStatusCode.Success)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpPost("register")]
        [SwaggerOperation(
        Summary = "Registers a new user.",
        Description = "Creates a new user account with provided information like email, username, and password."
        )]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
        {
            var response = await _mediator.Send(command);

            if (response.ResponseStatusCode == ResponseStatusCode.Success)
                return Ok(response);
            return BadRequest(response);
        }


    }
}
