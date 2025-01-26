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
        
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserCredentialsDto userCredentialsDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var token = await _authService.Login(userCredentialsDto);

            if (token == null)
                return Unauthorized();

            return Ok(token);
        }
    }
}