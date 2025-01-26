using Microsoft.EntityFrameworkCore;
using TaskTrackerWebAPI.Entities;

namespace TaskTrackerWebAPI.Services
{
    public class AuthService
    {
        private readonly JwtTokenService _jwtTokenService;
        private readonly TodoContext _context;

        public AuthService(JwtTokenService jwtTokenService, TodoContext context)
        {
            _jwtTokenService = jwtTokenService;
            _context = context;
            
            if (!_context.UsersCredentials.Any())
                _context.Database.EnsureCreated();
        }

        public async Task<AuthenticatedResponse?> Login(UserCredentialsDto userCredentialsDto)
        {
            var userCredentials = await _context.UsersCredentials.FirstOrDefaultAsync(u =>
                u.Login == userCredentialsDto.Email && u.Password == userCredentialsDto.Password);

            if (userCredentials == null)
                return null;
            
            return new AuthenticatedResponse
            {
                AccessToken = _jwtTokenService.CreateAccessToken(),
            };
        }
    }
}