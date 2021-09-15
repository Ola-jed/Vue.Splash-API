using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Vue.Splash_API.Data.Context;
using Vue.Splash_API.Data.Repositories;
using Vue.Splash_API.Extensions;
using Vue.Splash_API.Models;
using Vue.Splash_API.Services.Auth;
using Vue.Splash_API.Services.Mail;
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
        // Note : Some extensions are used to make the code more readable
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.ConfigurePgsql(Configuration);
            services.ConfigureBackblaze(Configuration);
            services.ConfigureMail(Configuration);
            services.ConfigureSwagger();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IStorageService, BackblazeStorageService>();
            services.AddScoped<IApplicationUserService, ApplicationUserService>();
            services.AddScoped<IPhotoRepository, PhotoRepository>();
            services.AddTransient<IMailService, MailService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedEmail = true)
                .AddEntityFrameworkStores<SplashContext>()
                .AddDefaultTokenProviders();
            services.AddMemoryCache();
            services.AddCors(options =>
            {
                options.AddPolicy(name: OriginsAllowed,
                    corsPolicyBuilder =>
                    {
                        corsPolicyBuilder
                            .AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader();
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

            app.UseRouting();
            app.UseCors(OriginsAllowed);
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}