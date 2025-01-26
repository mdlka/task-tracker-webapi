using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TaskTrackerWebAPI.Entities;
using TaskTrackerWebAPI.Services;

namespace TaskTrackerWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IOptions<JwtConfig> _config;

        public AuthController(IOptions<JwtConfig> config)
        {
            _config = config;
        }
        
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginCredentialsDto loginCredentialsDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (loginCredentialsDto is { Email: "test@mail.ru", Password: "123" })
            {
                var secretKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config.Value.Secret));
                var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                var tokenOptions = new JwtSecurityToken(
                    claims: new List<Claim>(),
                    expires: DateTime.Now.AddMinutes(5),
                    signingCredentials: signingCredentials
                );
                
                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
                return Ok(new LoginTokensDto {Access = tokenString});
            }

            return Unauthorized();
        }
    }
}