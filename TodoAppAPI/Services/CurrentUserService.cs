using System.Security.Claims;
using TodoAppApplication.Exceptions;
using TodoAppApplication.Abstractions;

namespace TodoAppApplication.Services
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