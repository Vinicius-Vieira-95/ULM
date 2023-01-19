using Microsoft.Extensions.DependencyInjection;

namespace UlmApi.Application.Extensions
{
    public static class CorsExtensions
    {
        public static void AddDefaultCorsPolicy(this IServiceCollection services)
        {
            services.AddCors(options => options.AddPolicy("DefaultPolicy",
            builder =>
            {
                builder.AllowAnyHeader()
                    .AllowAnyMethod()
                    .SetIsOriginAllowed((host) => true)
                    .AllowCredentials();
            }));
        }
    }
}
