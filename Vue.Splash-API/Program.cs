using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Vue.Splash_API.Extensions;
using Vue.Splash_API.Services.Auth;
using Vue.Splash_API.Services.Mail;
using Vue.Splash_API.Services.Storage;
using Vue.Splash_API.Services.Thumbnail;
using Vue.Splash_API.Services.User;
using Microsoft.AspNetCore.HttpLogging;
using Vue.Splash_API.Services.Photos;
using Vue.Splash_API.Services.UserPhotos;

const string originsAllowed = "_originsAllowed";

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options => { options.SuppressMapClientErrors = true; });
builder.Services.ConfigurePgsql(configuration);
builder.Services.ConfigureBackblaze(configuration);
builder.Services.ConfigureMail(configuration);
builder.Services.ConfigureBlobStorage(configuration);
builder.Services.ConfigureSwagger();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddSingleton<IStorageService, LocalStorageService>();
builder.Services.AddScoped<IApplicationUserService, ApplicationUserService>();
builder.Services.AddScoped<IPhotosService, PhotosService>();
builder.Services.AddScoped<IThumbnailService, ThumbnailService>();
builder.Services.AddScoped<IUserPhotosService, UserPhotosService>();
builder.Services.AddScoped<IMailService, MailService>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddMapper();
builder.Services.ConfigureIdentity();
builder.Services.AddMemoryCache();
builder.Services.ConfigureCors(originsAllowed);
builder.Services.ConfigureAuthentication(configuration);
builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.All;
    logging.RequestBodyLogLimit = 4096;
    logging.ResponseBodyLogLimit = 4096;
});
using var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Vue.Splash_API v1"));
}

app.UseRouting();
app.UseCors(originsAllowed);
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints => endpoints.MapControllers());
app.Run();