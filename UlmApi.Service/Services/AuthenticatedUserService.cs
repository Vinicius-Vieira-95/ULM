using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace UlmApi.Service.Services
{
    public class AuthenticatedUserService
    {
        private readonly IHttpContextAccessor _accessor;

        public AuthenticatedUserService(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public string Id => GetClaim("sub")?.Value;
        public string Role => GetHighestRole();
        public string FullName => $"{GetClaim("given_name")?.Value} {GetClaim("family_name")?.Value}";

        private Claim GetClaim(string claimName)
        {
            return _accessor.HttpContext.User.Claims.Where(c => c.Type == claimName).FirstOrDefault();
        }

        private string GetHighestRole()
        {
            var roles = _accessor.HttpContext.User.Claims
                                 .Where(c => c.Type == "user_roles")
                                 .Select(r => r.Value)
                                 .ToList();

            if (roles.Contains("ADMIN"))
                return "ADMIN";

            if (roles.Contains("OWNER"))
                return "OWNER";

            return "REQUESTER";

        }
    }
}
