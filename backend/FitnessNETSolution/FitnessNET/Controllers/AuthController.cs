using FitnessNET.Models.Auth;
using FitnessNET.Services;
using Microsoft.AspNetCore.Mvc;

namespace FitnessNET.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] ClientRegisterRequest request)
        {
            var (success, message, token) = await _authService.RegisterAsync(request);

            if (!success)
            {
                return BadRequest(new { Message = message });
            }

            return Ok(new { Token = token, Message = message });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] ClientLoginRequest request)
        {
            var (success, message, token) = await _authService.LoginAsync(request);

            if (!success)
            {
                return BadRequest(new { Message = message });
            }

            return Ok(new { Token = token, Message = message });
        }
    }
}
