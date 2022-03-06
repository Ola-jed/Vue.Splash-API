using System;
using System.Text;
using AutoMapper;
using Backblaze_Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Npgsql;
using Vue.Splash_API.Data;
using Vue.Splash_API.Profiles;
using Vue.Splash_API.Services.Auth;
using Vue.Splash_API.Services.EmailVerification;
using Vue.Splash_API.Services.ForgotPassword;
using Vue.Splash_API.Services.Mail;
using Vue.Splash_API.Services.Photos;
using Vue.Splash_API.Services.Storage;
using Vue.Splash_API.Services.Thumbnail;
using Vue.Splash_API.Services.User;
using Vue.Splash_API.Services.UserPhotos;

namespace Vue.Splash_API.Extensions;

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
        var cfg = configuration.GetSection("BackblazeConfig");
        cfg["KeyId"] = configuration["KeyId"];
        cfg["AppKey"] = configuration["AppKey"];
        cfg["BucketId"] = configuration["BucketId"];
        serviceCollection.Configure<BackblazeConfig>(cfg);
    }

    public static void ConfigurePgsql(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        var builder = new NpgsqlConnectionStringBuilder
        {
            ConnectionString = configuration.GetConnectionString("DefaultConnection"),
            Database = configuration["PgDbName"] ?? "splash",
            Host = configuration["PgHost"] ?? "127.0.0.1",
            Password = configuration["PgPassword"],
            Username = configuration["PgUserId"]
        };
        serviceCollection.AddDbContext<SplashContext>(opt => opt.UseNpgsql(builder.ConnectionString));
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
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
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

    public static void AddMapper(this IServiceCollection serviceCollection)
    {
        MapperConfiguration config = new(u =>
        {
            u.AddProfile<PhotoProfile>();
            u.AddProfile<ApplicationUserProfile>();
        });
        serviceCollection.AddSingleton(config.CreateMapper());
    }

    public static void AddServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IAuthService, AuthService>();
        serviceCollection.AddSingleton<IStorageService, BackblazeStorageService>();
        serviceCollection.AddScoped<IApplicationUserService, ApplicationUserService>();
        serviceCollection.AddScoped<IPhotosService, PhotosService>();
        serviceCollection.AddScoped<IThumbnailService, ThumbnailService>();
        serviceCollection.AddScoped<IUserPhotosService, UserPhotosService>();
        serviceCollection.AddScoped<IMailService, MailService>();
        serviceCollection.AddScoped<IEmailVerificationService, EmailVerificationService>();
        serviceCollection.AddScoped<IForgotPasswordService, ForgotPasswordService>();
        serviceCollection.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
    }
}