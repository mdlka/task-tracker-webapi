using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using TaskTrackerWebAPI.Entities;

namespace TaskTrackerWebAPI.Services
{
    public class AuthService
    {
        private readonly TokenService _tokenService;
        private readonly TodoContext _context;

        public AuthService(TokenService tokenService, TodoContext context)
        {
            _tokenService = tokenService;
            _context = context;
        }

        public async Task<bool> Register(RegistrationDto registrationDto)
        {
            if (await _context.Users.FirstOrDefaultAsync(u => u.Email == registrationDto.Email) != null)
                return false;

            var userId = Guid.NewGuid();

            await _context.Users.AddAsync(new User
            {
                Id = userId,
                Email = registrationDto.Email,
                Name = registrationDto.Name,
                CreatedAt = DateTime.UtcNow
            });
            
            await _context.UsersCredentials.AddAsync(new UserCredentials()
            {
                UserId = userId,
                Login = registrationDto.Email,
                Password = registrationDto.Password,
                Version = 1
            });

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<TokensDto?> Login(UserCredentialsDto userCredentialsDto)
        {
            var userCredentials = await _context.UsersCredentials.FirstOrDefaultAsync(u =>
                u.Login == userCredentialsDto.Email && u.Password == userCredentialsDto.Password);

            if (userCredentials == null)
                return null;

            var refreshToken = _tokenService.CreateRefreshToken();
            var oldRefreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.UserId == userCredentials.UserId);

            if (oldRefreshToken == null)
            {
                await _context.RefreshTokens.AddAsync(new RefreshToken
                {
                    UserId = userCredentials.UserId,
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
            
            return new TokensDto
            {
                AccessToken = _tokenService.CreateAccessToken(userCredentials.UserId),
                RefreshToken = refreshToken.Token
            };
        }

        public async Task<TokensDto?> Refresh(TokensDto tokens)
        {
            var principal = _tokenService.GetPrincipalFromExpiredToken(tokens.AccessToken);
            var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return null;
            
            var userId = Guid.Parse(userIdClaim.Value);
            var oldRefreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(token => token.UserId == userId);

            if (oldRefreshToken == null)
                return null;

            if (DateTime.Now > oldRefreshToken.ExpiresAt)
                return null;

            var newRefreshToken = _tokenService.CreateRefreshToken();
            oldRefreshToken.Token = newRefreshToken.Token;
            oldRefreshToken.ExpiresAt = newRefreshToken.ExpiresAt;

            _context.RefreshTokens.Update(oldRefreshToken);
            await _context.SaveChangesAsync();

            return new TokensDto
            {
                AccessToken = _tokenService.CreateAccessToken(oldRefreshToken.UserId),
                RefreshToken = newRefreshToken.Token
            };
        }
    }
}