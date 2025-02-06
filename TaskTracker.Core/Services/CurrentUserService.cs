using System.Security.Claims;

namespace TaskTracker.Core.Services
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
            
            var userIdClaim = _user.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                throw new InvalidOperationException();
            
            return (_userId = Guid.Parse(userIdClaim.Value)).Value;
        }
    }
}