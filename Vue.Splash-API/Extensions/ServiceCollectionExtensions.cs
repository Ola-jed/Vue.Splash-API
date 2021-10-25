using System;
using System.Text;
using Backblaze_Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Npgsql;
using Vue.Splash_API.Data.Context;
using Vue.Splash_API.Models;
using Vue.Splash_API.Services.Mail;
using Vue.Splash_API.Services.Storage;

namespace Vue.Splash_API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureMail(this IServiceCollection serviceCollection,
            IConfiguration configuration)
        {
            var cfg = configuration.GetSection("MailSettings");
            cfg["Host"] = configuration["MailHost"];
            cfg["Port"] = configuration["MailPort"];
            cfg["MailUser"] = configuration["MailUser"];
            cfg["MailPassword"] = configuration["MailPassword"];
            serviceCollection.Configure<MailSettings>(cfg);
        }

        public static void ConfigureBlobStorage(this IServiceCollection serviceCollection,
            IConfiguration configuration)
        {
            var cfg = configuration.GetSection("BlobStorage");
            cfg["AzureBlobKey"] = configuration["AzureBlobKey"];
            cfg["ContainerName"] = configuration["ContainerName"];
            serviceCollection.Configure<BlobStorageSettings>(cfg);
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

        public static void ConfigureIdentity(this IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedEmail = true)
                .AddEntityFrameworkStores<SplashContext>()
                .AddDefaultTokenProviders();
        }

        public static void ConfigureAuthentication(this IServiceCollection serviceCollection,
            IConfiguration configuration)
        {
            serviceCollection.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidAudience = configuration["JWT:ValidAudience"],
                        ValidIssuer = configuration["JWT:ValidIssuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
                    };
                });
        }

        public static void ConfigureCors(this IServiceCollection serviceCollection,
            string policyName)
        {
            serviceCollection.AddCors(options =>
            {
                options.AddPolicy(policyName,
                    corsPolicyBuilder =>
                    {
                        corsPolicyBuilder
                            .AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                    });
            });
        }

        public static void ConfigureSwagger(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Vue.Splash",
                    Version = "v1",
                    Description = "An api for an unsplash clone",
                    Contact = new OpenApiContact
                    {
                        Name = "Olabissi Gbangboche",
                        Email = "olabijed@gmail.com",
                        Url = new Uri("https://github.com/Ola-jed")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under MIT",
                        Url = new Uri("https://github.com/Ola-jed/Vue.Splash-API/blob/master/LICENSE")
                    }
                });
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