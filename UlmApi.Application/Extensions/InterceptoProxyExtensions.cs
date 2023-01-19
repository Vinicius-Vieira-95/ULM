using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using UlmApi.Domain.Entities;
using UlmApi.Domain.Interfaces;
using UlmApi.Service.Interceptors;
using UlmApi.Service.Services;

namespace UlmApi.Application.Extensions
{
    public static class InterceptoProxyExtensions
    {
        public static void AddProxies(this IServiceCollection services)
        {
            services.AddScoped(serviceProvider =>
            {
                var proxy = InterceptDecorator<IRequestLicenseService>.Create(
                    new RequestLicenseService(serviceProvider.GetService<IRequestLicenseRepository>(), 
                        serviceProvider.GetService<ILicenseRepository>(), 
                        serviceProvider.GetService<IMapper>(), 
                        serviceProvider.GetService<IBaseRepository<Product, int>>(),  
                        serviceProvider.GetService<IBaseRepository<UlmApi.Domain.Entities.Application, int>>(), 
                        serviceProvider.GetService<AuthenticatedUserService>(), 
                        serviceProvider.GetService<IUserService>(), 
                        serviceProvider.GetService<ISolutionRepository>()), serviceProvider);

                return proxy;
                    
            });

            services.AddScoped(serviceProvider =>
            {
                var proxy = InterceptDecorator<ILicenseService>.Create(
                    new LicenseService(serviceProvider.GetService<ILicenseRepository>(), 
                        serviceProvider.GetService<IMapper>(), 
                        serviceProvider.GetService<IBaseRepository<UlmApi.Domain.Entities.Application, int>>(),
                        serviceProvider.GetService<AuthenticatedUserService>(),
                        serviceProvider.GetService<ISolutionRepository>(),
                        serviceProvider.GetService<IBaseRepository<Solution, int>>()), serviceProvider);

                return proxy;
                    
            });

            services.AddScoped(serviceProvider =>
            {
                var proxy = InterceptDecorator<IApplicationService>.Create(
                    new ApplicationService(serviceProvider.GetService<IApplicationRepository>(),  
                        serviceProvider.GetService<IMapper>()), serviceProvider);

                return proxy;
                    
            });

            services.AddScoped(serviceProvider =>
            {
                var proxy = InterceptDecorator<ISolutionService>.Create(
                    new SolutionService(serviceProvider.GetService<ISolutionRepository>(), 
                        serviceProvider.GetService<IMapper>(), 
                        serviceProvider.GetService<IUserService>(),
                        serviceProvider.GetService<AuthenticatedUserService>(),
                        serviceProvider.GetService<IBaseService<Product, int>>()), serviceProvider);

                return proxy;
                    
            });

            services.AddScoped(serviceProvider =>
            {
                var proxy = InterceptDecorator<IUserService>.Create(
                    new UserService(serviceProvider.GetService<ISolutionRepository>(), 
                        serviceProvider.GetService<IMapper>(), 
                        serviceProvider.GetService<AuthenticatedUserService>()), serviceProvider);

                return proxy;
                    
            });
        }
    }
}
