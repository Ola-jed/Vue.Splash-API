using System;
using System.Text;
using Backblaze_Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Npgsql;
using Vue.Splash_API.Data.Context;
using Vue.Splash_API.Data.Repositories;
using Vue.Splash_API.Models;
using Vue.Splash_API.Services.Auth;
using Vue.Splash_API.Services.Storage;
using Vue.Splash_API.Services.User;

namespace Vue.Splash_API
{
    public class Startup
    {
        private readonly string OriginsAllowed = "_originsAllowed";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            var config = new BackblazeConfig()
            {
                KeyId = Configuration["KeyId"],
                AppKey = Configuration["AppKey"],
                BucketId = Configuration["BucketId"],
                ApiBase = "https://api.backblazeb2.com/b2api/v2/"
            };
            services.AddSingleton(config);
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IStorageService, BackblazeStorageService>();
            services.AddScoped<IApplicationUserService, ApplicationUserService>();
            services.AddScoped<IPhotoRepository, PhotoRepository>();
            services.AddSwaggerGen(c =>
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
            var builder = new NpgsqlConnectionStringBuilder
            {
                ConnectionString = Configuration.GetConnectionString("DefaultConnection"),
                Password = Configuration["Password"],
                Username = Configuration["UserId"]
            };
            services.AddDbContext<SplashContext>(opt => opt.UseNpgsql(builder.ConnectionString));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<SplashContext>()
                .AddDefaultTokenProviders();
            services.AddMemoryCache();
            services.AddCors(options =>
            {
                options.AddPolicy(name: OriginsAllowed,
                    corsPolicyBuilder =>
                    {
                        corsPolicyBuilder.WithOrigins("http://localhost:3000",
                            "http://localhost:8080");
                    });
            });
            services.AddAuthentication(options =>
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
                        ValidAudience = Configuration["JWT:ValidAudience"],
                        ValidIssuer = Configuration["JWT:ValidIssuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"]))
                    };
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Vue.Splash_API v1"));
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseRouting();
            app.UseCors(OriginsAllowed);
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}