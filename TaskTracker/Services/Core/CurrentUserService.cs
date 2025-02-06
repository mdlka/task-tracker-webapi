using System.Security.Claims;

namespace TaskTracker.Services
{
    public class CurrentUserService
    {
        private readonly ClaimsPrincipal _user;
        private Guid? _userId;

        public CurrentUserService(ClaimsPrincipal user)
        {
            _user = user;
        }

        public bool IsAnonymous => _user.FindFirst(ClaimTypes.NameIdentifier) == null;

        public Guid GetUserId()
        {
            if (_userId != null)
                return _userId.Value;
            
            string? userId = _user.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
                throw new InvalidOperationException();
            
            return (_userId = Guid.Parse(userId)).Value;
        }
    }
}