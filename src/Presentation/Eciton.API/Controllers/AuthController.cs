using Eciton.Application.Abstractions;
using Eciton.Application.DTOs.Auth;
using Eciton.Application.ResponceObject.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Eciton.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            var response = await _authService.LoginAsync(loginDto);

            if (response.ResponseStatusCode == ResponseStatusCode.Success)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto)
        {
            var response = await _authService.RegisterAsync(registerDto);

            if (response.ResponseStatusCode == ResponseStatusCode.Success)
                return Ok(response);
            return BadRequest(response);
        }


    }
}
