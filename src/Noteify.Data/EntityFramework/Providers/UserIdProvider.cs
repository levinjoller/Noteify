using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Noteify.Data.EntityFramework.Providers
{
    public class UserIdProvider
    {
        private readonly IHttpContextAccessor _accessor;

        public UserIdProvider(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public string GetUserId()
        {
            return _accessor.HttpContext?.User
                .FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}