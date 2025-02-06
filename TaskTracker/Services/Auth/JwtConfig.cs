namespace TaskTracker.Services
{
    public class JwtConfig
    {
        public string Secret { get; set; }
        public string AccessTokenExpirationMinutes { get; set; }
        public string RefreshTokenExpirationDays { get; set; }

        public TimeSpan AccessTokenLifetime => TimeSpan.FromMinutes(int.Parse(AccessTokenExpirationMinutes));
        public TimeSpan RefreshTokenLifetime => TimeSpan.FromDays(int.Parse(RefreshTokenExpirationDays));
    }
}