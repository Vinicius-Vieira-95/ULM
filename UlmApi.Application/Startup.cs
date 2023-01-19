using UlmApi.Infra.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AutoMapper;
using UlmApi.Application.Models;
using UlmApi.Domain.Entities;
using UlmApi.Domain.Interfaces;
using UlmApi.Service.Services;
using FluentValidation.AspNetCore;
using UlmApi.Service.Validators;
using UlmApi.Application.Extensions;
using UlmApi.Infra.CrossCutting.Logger;
using UlmApi.Service.Interceptors;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using UlmApi.Infra.CrossCutting.RabbitMQ.Consumers;

namespace UlmApi.Application
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<Context>();
            services.AddDependencyInjections();
            services.AddDefaultCorsPolicy();
            services.AddRabbitMQConnection();
            services.AddDefaultAuthorization();
            services.AddJwtAuthentication();
            services.AddProxies();
            services.AddHostedService<ProcessIAResultConsumer>();

            services.AddControllers()
                .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
                .AddFluentValidation(p => 
                {
                    p.RegisterValidatorsFromAssemblyContaining<GetLicensesPaginationQueryValidator>();
                    p.RegisterValidatorsFromAssemblyContaining<GetLicensesBySolutionQueryValidator>();
                    p.RegisterValidatorsFromAssemblyContaining<GetSolutionsQueryValidator>();
                    p.RegisterValidatorsFromAssemblyContaining<UpdateUserRoleValidator>();
                    p.RegisterValidatorsFromAssemblyContaining<ChangePasswordValidator>();
                });

            services.AddSingleton(new MapperConfiguration(config =>
            {
                config.CreateMap<Domain.Entities.Application, ApplicationModel>();
                config.CreateMap<Solution, SolutionModel>();
                config.CreateMap<Product, ProductModel>();
                config.CreateMap<CreateRequestLicenseModel, RequestLicense>();
                config.CreateMap<CreateLicenseModel, License>();
                config.CreateMap<CreateSolutionModel, Solution>();
                config.CreateMap<CreateApplicationModel, Domain.Entities.Application>();
            }).CreateMapper());

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "ULM-API",
                    Version = "v1"
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
            });

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("DefaultPolicy");
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(opt =>
            {
                opt.SwaggerEndpoint("/swagger/v1/swagger.json", "ULM API V1");
                opt.RoutePrefix = string.Empty;
            });
        }
    }
}
