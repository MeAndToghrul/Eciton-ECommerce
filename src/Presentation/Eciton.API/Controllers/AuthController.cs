using Eciton.Application.Abstractions;
using Eciton.Application.Commands.Auth;
using Eciton.Application.DTOs.Auth;
using Eciton.Application.ResponceObject.Enums;
using Eciton.Persistence.Implements;
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

        [HttpGet("verify-email")]
        [SwaggerOperation(
            Summary = "Verifies user's email address.",
            Description = "This endpoint is used to confirm user's email address via a verification token sent to their email."
        )]
        public async Task<IActionResult> VerifyEmail([FromQuery] string token)
        {
            var command = new VerifyEmailCommand { Token = token };
            var response = await _mediator.Send(command);

            if (response.ResponseStatusCode == ResponseStatusCode.Success)
                return Ok(response);

            return BadRequest(response);
        }

        [HttpPost("resend-email-verification")]
        [SwaggerOperation(
        Summary = "Resends the email verification link.",
        Description = "This endpoint resends a verification email to the user if their email is not yet confirmed."
        )]
        public async Task<IActionResult> ResendEmailVerification([FromBody] ResendEmailVerificationCommand command)
        {
            var response = await _mediator.Send(command);

            if (response.ResponseStatusCode == ResponseStatusCode.Success)
                return Ok(response);

            return BadRequest(response);
        }

        [HttpPost("reset-password")]
        [SwaggerOperation(
    Summary = "Sends a password reset email.",
    Description = "Generates a password reset token and sends it to the user's email."
)]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestCommand command)
        {
            var response = await _mediator.Send(command);

            if (response.ResponseStatusCode == ResponseStatusCode.Success)
                return Ok(response);

            return BadRequest(response);
        }

        [HttpPost("confirm-reset-password")]
        [SwaggerOperation(
            Summary = "Confirms password reset with token and new password.",
            Description = "Accepts token, new password and confirmation to reset the user's password."
        )]
        public async Task<IActionResult> ConfirmResetPassword([FromBody] ConfirmResetPasswordCommand command)
        {
            var response = await _mediator.Send(command);

            if (response.ResponseStatusCode == ResponseStatusCode.Success)
                return Ok(response);

            return BadRequest(response);
        }
    }
}
