using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace TaskTrackerWebAPI.Services
{
    public class JwtTokenService
    {
        private const string SecurityAlgorithm = SecurityAlgorithms.HmacSha256;

        private readonly IOptions<JwtConfig> _config;

        public JwtTokenService(IOptions<JwtConfig> jwtConfig)
        {
            _config = jwtConfig;
        }

        public string CreateAccessToken(string id)
        {
            var claims = new List<Claim>
            {
                new Claim("id", id),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.Value.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithm);

            var tokenDescriptor = new SecurityTokenDescriptor()
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
                
            return new RefreshToken()
            {
                Token = Convert.ToBase64String(randomNumber),
                ExpiresAt = DateTime.UtcNow.Add(_config.Value.RefreshTokenLifetime)
            };
        }

        public struct RefreshToken
        {
            public string Token { get; set;}
            public DateTime ExpiresAt { get; set;}
        }
    }
    
    public class JwtConfig
    {
        public string Secret { get; set; }
        public string accessTokenExpirationMinutes { get; set; }
        public string refreshTokenExpirationDays { get; set; }

        public TimeSpan AccessTokenLifetime => TimeSpan.FromMinutes(int.Parse(accessTokenExpirationMinutes));
        public TimeSpan RefreshTokenLifetime => TimeSpan.FromDays(int.Parse(refreshTokenExpirationDays));
    }
}