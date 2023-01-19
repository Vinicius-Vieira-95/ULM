using Microsoft.Extensions.DependencyInjection;
using UlmApi.Domain.Entities;
using UlmApi.Domain.Interfaces;
using UlmApi.Infra.CrossCutting.RabbitMQ;
using UlmApi.Infra.CrossCutting.Logger;
using UlmApi.Infra.Data.Repository;
using UlmApi.Service.Services;
using Microsoft.AspNetCore.Http;

namespace UlmApi.Application.Extensions
{
    public static class DependencyInjections
    {
        public static void AddDependencyInjections(this IServiceCollection services)
        {
            //Repositories
            services.AddScoped<ILicenseRepository, LicenseRepository>();
            services.AddScoped<IBaseRepository<RequestLicense, int>, BaseRepository<RequestLicense, int>>();
            services.AddScoped<IBaseRepository<Domain.Entities.Application, int>, BaseRepository<Domain.Entities.Application, int>>();
            services.AddScoped<IBaseRepository<Product, int>, BaseRepository<Product, int>>();
            services.AddScoped<IBaseRepository<Solution, int>, BaseRepository<Solution, int>>();
            services.AddScoped<IRequestLicenseRepository, RequestLicenseRepository>();
            services.AddScoped<ISolutionRepository, SolutionRepository>();
            services.AddScoped<IApplicationRepository, ApplicationRepository>();
            services.AddScoped<IDashboardRepository, DashboardRepository>();

            //Services
            services.AddScoped<ILicenseService, LicenseService>();
            services.AddScoped<IBaseService<RequestLicense, int>, BaseService<RequestLicense, int>>();
            services.AddScoped<IRequestLicenseService, RequestLicenseService>();
            services.AddScoped<IBaseService<Domain.Entities.Application, int>, BaseService<Domain.Entities.Application, int>>();
            services.AddScoped<IBaseService<Product, int>, BaseService<Product, int>>();
            services.AddScoped<IBaseService<Solution, int>, BaseService<Solution, int>>();
            services.AddScoped<IRequestLicenseService, RequestLicenseService>();
            services.AddScoped<IEventBus, EventBusRabbitMQ>();
            services.AddScoped<ILoggerManager, LoggerManager>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ISolutionService, SolutionService>();
            services.AddScoped<IApplicationService, ApplicationService>();
            services.AddScoped<AuthenticatedUserService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IDashboardService, DashboardService>();
        }
    }
}
