using Microsoft.AspNetCore.Mvc;
using TaskTracker.Entities;
using TaskTracker.Services;

namespace TaskTracker.Controllers
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
            return await _authService.Register(registrationDto.Email, registrationDto.Name, registrationDto.Password) 
                ? Ok() 
                : BadRequest();
        }
        
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserCredentialsDto userCredentials)
        {
            var token = await _authService.Login(userCredentials.Email, userCredentials.Password);
            return Ok(token);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] Tokens tokens)
        {
            var token = await _authService.Refresh(tokens);

            if (token == null)
                return BadRequest();

            return Ok(token);
        }
    }
}