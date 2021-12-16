using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using POS.Business.Data;
using POS.Business.Interfaces;
using POS.Common.Security;
using POS.Data.Context;
using POS.Repository.Data;
using POS.Repository.Infrastructure;
using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace POS.API.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config["ConnectionStrings:POS_DB"];
            services.AddDbContext<RepositoryContext>(o => o.UseSqlServer(connectionString));
        }

        public static void ConfigureRepositoryWrapper(this IServiceCollection services)
        {
            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();

        }
        public static void ConfigureLoggerService(this IServiceCollection services)
        {
            services.AddSingleton<ILoggerManager, LoggerManager>();
        }

        public static void ConfigureCustomLogExtensions(this IApplicationBuilder app)
        {
            app.UseMiddleware<CustomLogExtensions>();
        }

        public static void ConfigureAppSettings(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<AppSettings>(config.GetSection("AppSettings"));
        }

        public static void ConfigureAuthentication(this IServiceCollection services, IConfiguration config)
        {
            string jwtKey = config["AppSettings:Jwt:Key"];
            int jwtExpiry = Convert.ToInt32(config["AppSettings:Jwt:Expiry"]);
            //string jwtIssuer = config["AppSettings:Jwt:Issuer"];
            //string jwtAudience = config["AppSettings:Jwt:Audience"];

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,                    
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,

                    //ValidIssuer = jwtIssuer,
                    //ValidAudience = jwtAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtKey)),
                };
            });

            services.AddSingleton<IAuthManager>(new AuthManager(jwtKey, jwtExpiry));

        }

        public static void ConfigureSwagger(this IServiceCollection services)
        {

            services.AddSwaggerGen(x =>
            {
                x.EnableAnnotations();

                x.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "This is title",
                    Description = "This is description",
                    Version = "v1"
                });

                x.AddSecurityDefinition("basic", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "basic",
                    In = ParameterLocation.Header,
                    Description = "Basic Auth Header"
                });

                x.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "basic"
                            }
                        },
                        new string[]{}
                    }
                });

                var fileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var filePath = Path.Combine(AppContext.BaseDirectory, fileName);
                x.IncludeXmlComments(filePath);

                string[] methodsOrder = new string[7] { "get", "post", "put", "patch", "delete", "options", "trace" };
                x.OrderActionsBy((apiDesc) => $"{apiDesc.RelativePath.Replace("/", "|")}_{Array.IndexOf(methodsOrder, apiDesc.HttpMethod.ToLower())}");
            });

        }

        public static void ConfigureApiVersioning(this IServiceCollection services)
        {
            services.AddVersionedApiExplorer(x =>
            {
                x.GroupNameFormat = "'v'VVV";
                x.SubstituteApiVersionInUrl = true;
                x.AssumeDefaultVersionWhenUnspecified = true;
                x.DefaultApiVersion = new ApiVersion(1, 0);
            });

            services.AddApiVersioning(x =>
            {
                x.ReportApiVersions = true;
                x.AssumeDefaultVersionWhenUnspecified = true;
                x.DefaultApiVersion = new ApiVersion(1, 0);
            });
        }
    }
}
