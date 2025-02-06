using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TaskTracker.Infrastructure.Options;

namespace TaskTracker.Infrastructure.Services
{
    public class TokenService
    {
        private const string SecurityAlgorithm = SecurityAlgorithms.HmacSha256;
        private readonly IOptions<JwtConfig> _config;

        public TokenService(IOptions<JwtConfig> jwtConfig)
        {
            _config = jwtConfig;
        }

        public string CreateAccessToken(Guid userId)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.Value.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithm);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            };
            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(_config.Value.AccessTokenLifetime),
                SigningCredentials = credentials,
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public RefreshToken CreateRefreshToken()
        {
            byte[] randomNumber = new byte[64];

            using var randomGenerator = RandomNumberGenerator.Create();
            randomGenerator.GetBytes(randomNumber);
                
            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomNumber),
                ExpiresAt = DateTime.UtcNow.Add(_config.Value.RefreshTokenLifetime)
            };
        }
        
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = false,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.Value.Secret))
            };
            
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken 
                || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithm, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");
            
            return principal;
        }

        public struct RefreshToken
        {
            public string Token { get; set;}
            public DateTime ExpiresAt { get; set;}
        }
    }
}