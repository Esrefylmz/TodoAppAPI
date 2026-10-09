using System.Security.Claims;
using TodoAppAPI.ExceptionHandling.Exceptions;

namespace TodoAppAPI.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(
            IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int UserId
        {
            get
            {
                var userIdValue = _httpContextAccessor
                    .HttpContext?
                    .User
                    .FindFirst(ClaimTypes.NameIdentifier)?
                    .Value;

                if (!int.TryParse(userIdValue, out var userId))
                {
                    throw new UnauthorizedException(
                        "User identity could not be determined.");
                }

                return userId;
            }
        }
    }
}