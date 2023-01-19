using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace UlmApi.Application.Extensions
{
    public static class AuthorizationExtensions
    {
        public static void AddDefaultAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(o =>
            {
                var adminPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .RequireClaim("user_roles", "ADMIN")
                    .Build();
                    o.AddPolicy(Policies.ADMIN, adminPolicy);

                var ownerPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .RequireClaim("user_roles", "OWNER")
                    .Build();
                    o.AddPolicy(Policies.OWNER, ownerPolicy);
                
                var requesterPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .RequireClaim("user_roles", "REQUESTER")
                    .Build();
                    o.AddPolicy(Policies.REQUESTER, requesterPolicy);

                var adminOrOwnerPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .RequireClaim("user_roles", new string[] {"OWNER", "ADMIN"})
                    .Build();
                    o.AddPolicy(Policies.ADMIN_OR_OWNER, adminOrOwnerPolicy);
    
            });
        }
    }
}
