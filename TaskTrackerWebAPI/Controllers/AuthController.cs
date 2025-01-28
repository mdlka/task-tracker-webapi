using Microsoft.AspNetCore.Mvc;
using TaskTrackerWebAPI.Entities;
using TaskTrackerWebAPI.Services;

namespace TaskTrackerWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationDto registrationDto)
        {
            return await _authService.Register(registrationDto) ? Ok() : BadRequest();
        }
        
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserCredentialsDto userCredentialsDto)
        {
            var token = await _authService.Login(userCredentialsDto);

            if (token == null)
                return Unauthorized();

            return Ok(token);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] TokensDto tokensDto)
        {
            var token = await _authService.Refresh(tokensDto);

            if (token == null)
                return BadRequest();

            return Ok(token);
        }
    }
}