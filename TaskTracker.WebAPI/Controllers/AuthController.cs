using Microsoft.AspNetCore.Mvc;
using TaskTracker.Core.Models;
using TaskTracker.Infrastructure.Services;
using TaskTracker.WebAPI.Dto;

namespace TaskTracker.WebAPI.Controllers
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
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            return await _authService.Register(registerRequest.Email, registerRequest.Name, registerRequest.Password) 
                ? Ok() 
                : BadRequest();
        }
        
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var token = await _authService.Login(loginRequest.Email, loginRequest.Password);
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