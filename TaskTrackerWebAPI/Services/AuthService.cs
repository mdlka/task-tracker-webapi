using TaskTrackerWebAPI.Entities;

namespace TaskTrackerWebAPI.Services
{
    public class AuthService
    {
        private readonly JwtTokenService _jwtTokenService;

        public AuthService(JwtTokenService jwtTokenService)
        {
            _jwtTokenService = jwtTokenService;
        }

        public LoginTokensDto? Login(LoginCredentialsDto loginCredentialsDto)
        {
            if (loginCredentialsDto is { Email: "test@mail.ru", Password: "123" })
            {
                return new LoginTokensDto
                {
                    Access = _jwtTokenService.CreateAccessToken()
                };
            }

            return null;
        }
    }
}