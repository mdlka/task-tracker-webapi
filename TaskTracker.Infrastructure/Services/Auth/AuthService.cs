using System.Security.Claims;
using TaskTracker.Core.Exceptions;
using TaskTracker.Core.Models;
using TaskTracker.Core.Repositories;

namespace TaskTracker.Infrastructure.Services
{
    public class AuthService
    {
        private readonly TokenService _tokenService;
        private readonly IAuthRepositoryWrapper _repositoryWrapper;

        public AuthService(IAuthRepositoryWrapper repositoryWrapper, TokenService tokenService)
        {
            _tokenService = tokenService;
            _repositoryWrapper = repositoryWrapper;
        }

        public async Task<bool> Register(string email, string name, string password)
        {
            if (await _repositoryWrapper.Users.FirstOrDefault(u => u.Email == email) != null)
                return false;

            var userId = Guid.NewGuid();

            await _repositoryWrapper.Users.Add(new User
            {
                Id = userId,
                Email = email,
                Name = name,
                CreatedAt = DateTime.UtcNow
            });
            
            await _repositoryWrapper.UserCredentials.Add(new UserCredentials
            {
                UserId = userId,
                Login = email,
                PasswordHash = password,
                Version = 1
            });

            await _repositoryWrapper.Save();

            return true;
        }

        public async Task<Tokens> Login(string email, string password)
        {
            var userCredentials = await _repositoryWrapper.UserCredentials
                .FirstOrDefault(u => u.Login == email);

            if (userCredentials == null || password != userCredentials.PasswordHash)
                throw new UnauthorizedException();

            var refreshToken = _tokenService.CreateRefreshToken();
            var oldRefreshToken = await _repositoryWrapper.RefreshTokens.FirstOrDefault(rt => rt.UserId == userCredentials.UserId);

            if (oldRefreshToken == null)
            {
                await _repositoryWrapper.RefreshTokens.Add(new RefreshToken
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
                _repositoryWrapper.RefreshTokens.Update(oldRefreshToken);
            }

            await _repositoryWrapper.Save();
            
            return new Tokens
            {
                AccessToken = _tokenService.CreateAccessToken(userCredentials.UserId),
                RefreshToken = refreshToken.Token
            };
        }

        public async Task<Tokens?> Refresh(Tokens tokens)
        {
            var principal = _tokenService.GetPrincipalFromExpiredToken(tokens.AccessToken);
            var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return null;
            
            var userId = Guid.Parse(userIdClaim.Value);
            var oldRefreshToken = await _repositoryWrapper.RefreshTokens.FirstOrDefault(token => token.UserId == userId);

            if (oldRefreshToken == null)
                return null;

            if (DateTime.Now > oldRefreshToken.ExpiresAt)
                return null;

            var newRefreshToken = _tokenService.CreateRefreshToken();
            oldRefreshToken.Token = newRefreshToken.Token;
            oldRefreshToken.ExpiresAt = newRefreshToken.ExpiresAt;

            _repositoryWrapper.RefreshTokens.Update(oldRefreshToken);
            await _repositoryWrapper.Save();

            return new Tokens
            {
                AccessToken = _tokenService.CreateAccessToken(oldRefreshToken.UserId),
                RefreshToken = newRefreshToken.Token
            };
        }
    }
}