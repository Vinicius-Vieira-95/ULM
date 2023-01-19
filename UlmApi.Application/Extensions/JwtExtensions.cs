using System;
using System.Net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace UlmApi.Application.Extensions
{
    public static class JwtExtensions
    {
        private static readonly string KEYCLOAK_URL = Environment.GetEnvironmentVariable("KEYCLOAK_URL") ?? "https://ulmauth.leadfortaleza.com.br";
        private static readonly string REALM = Environment.GetEnvironmentVariable("KEYCLOAK_REALM") ?? "ulm";
        private static readonly string CLIENT = Environment.GetEnvironmentVariable("KEYCLOAK_CLIENT") ?? "ulm-frontend";

        public static void AddJwtAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                o.RequireHttpsMetadata = false;
                o.Authority = $"{KEYCLOAK_URL}/auth/realms/{REALM}";
                o.Audience = CLIENT;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidAudiences = new string[] {"account", CLIENT}
                };
            });
        }
    }
}
