using System.Security.Claims;

namespace TaskTrackerWebAPI.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static bool TryGetUserId(this ClaimsPrincipal user, out Guid userId)
        {
            userId = Guid.Empty;
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return false;

            userId = Guid.Parse(userIdClaim.Value);
            return true;
        }
    }
}