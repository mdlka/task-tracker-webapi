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

            var refreshToken = _jwtTokenService.CreateRefreshToken();
            var oldRefreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Id == userCredentials.Id);

            if (oldRefreshToken == null)
            {
                await _context.RefreshTokens.AddAsync(new RefreshToken
                {
                    Id = userCredentials.Id,
                    Token = refreshToken.Token,
                    ExpiresAt = refreshToken.ExpiresAt
                });
            }
            else
            {
                oldRefreshToken.Token = refreshToken.Token;
                oldRefreshToken.ExpiresAt = refreshToken.ExpiresAt;
                _context.RefreshTokens.Update(oldRefreshToken);
            }

            await _context.SaveChangesAsync();
            
            return new AuthenticatedResponse
            {
                AccessToken = _jwtTokenService.CreateAccessToken(),
                RefreshToken = refreshToken.Token
            };
        }
    }
}