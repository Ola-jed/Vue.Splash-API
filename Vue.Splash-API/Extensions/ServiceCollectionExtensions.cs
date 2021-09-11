using Backblaze_Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Npgsql;
using Vue.Splash_API.Data.Context;
using Vue.Splash_API.Services.Mail;

namespace Vue.Splash_API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureMail(this IServiceCollection serviceCollection,
            IConfiguration configuration)
        {
            var cfg = configuration.GetSection("MailSettings");
            cfg["MailUser"] = configuration["MailUser"];
            cfg["MailPassword"] = configuration["MailPassword"];
            serviceCollection.Configure<MailSettings>(cfg);
        }

        public static void ConfigureBackblaze(this IServiceCollection serviceCollection,
            IConfiguration configuration)
        {
            var config = new BackblazeConfig()
            {
                KeyId = configuration["KeyId"],
                AppKey = configuration["AppKey"],
                BucketId = configuration["BucketId"],
                ApiBase = "https://api.backblazeb2.com/b2api/v2/"
            };
            serviceCollection.AddSingleton(config);
        }

        public static void ConfigurePgsql(this IServiceCollection serviceCollection,
            IConfiguration configuration)
        {
            var builder = new NpgsqlConnectionStringBuilder
            {
                ConnectionString = configuration.GetConnectionString("DefaultConnection"),
                Password = configuration["Password"],
                Username = configuration["UserId"]
            };
            serviceCollection.AddDbContext<SplashContext>(opt => opt.UseNpgsql(builder.ConnectionString));
        }

        public static void ConfigureSwagger(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Vue.Splash_API", Version = "v1" });
                var securitySchema = new OpenApiSecurityScheme
                {
                    Description = "Authorization header using the Bearer scheme. (\"Authorization: Bearer {token}\")",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                };
                c.AddSecurityDefinition("Bearer", securitySchema);
                var securityRequirement = new OpenApiSecurityRequirement
                {
                    { securitySchema, new[] { "Bearer" } }
                };
                c.AddSecurityRequirement(securityRequirement);
            });
        }
    }
}