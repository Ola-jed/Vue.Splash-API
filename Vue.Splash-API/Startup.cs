using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Vue.Splash_API.Data.Repositories;
using Vue.Splash_API.Extensions;
using Vue.Splash_API.Services.Auth;
using Vue.Splash_API.Services.Mail;
using Vue.Splash_API.Services.Storage;
using Vue.Splash_API.Services.Thumbnail;
using Vue.Splash_API.Services.User;

namespace Vue.Splash_API
{
    public class Startup
    {
        private const string OriginsAllowed = "_originsAllowed";

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
            services.ConfigureBlobStorage(Configuration);
            services.ConfigureSwagger();
            services.AddScoped<IAuthService, AuthService>();
            services.AddSingleton<IStorageService, LocalStorageService>();
            services.AddScoped<IApplicationUserService, ApplicationUserService>();
            services.AddScoped<IPhotoRepository, PhotoRepository>();
            services.AddScoped<IThumbnailService, ThumbnailService>();
            services.AddTransient<IMailService, MailService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.ConfigureIdentity();
            services.AddMemoryCache();
            services.ConfigureCors(OriginsAllowed);
            services.ConfigureAuthentication(Configuration);
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
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
