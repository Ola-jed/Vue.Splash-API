using System;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Vue.Splash_API.Extensions;
using Microsoft.Extensions.Logging;
using Serilog;

const string originsAllowed = "_originsAllowed";

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.File($"logs/{Assembly.GetExecutingAssembly().GetName().Name}.log")
    .WriteTo.Console()
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog();
builder.Services.AddControllers().ConfigureApiBehaviorOptions(options => options.SuppressMapClientErrors = true);
builder.Services.ConfigurePgsql(configuration);
builder.Services.ConfigureBackblaze(configuration);
builder.Services.ConfigureMail(configuration);
builder.Services.ConfigureBlobStorage(configuration);
builder.Services.ConfigureSwagger();
builder.Services.AddServices();
builder.Services.AddMapper();
builder.Services.AddMemoryCache();
builder.Services.ConfigureCors(originsAllowed);
builder.Services.ConfigureAuthentication(configuration);

using var app = builder.Build();
if (app.Environment.IsProduction())
{
    var port = Environment.GetEnvironmentVariable("PORT");
    app.Urls.Add($"http://*:{port}");
}

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Vue.Splash_API v1"));
app.UseRouting();
app.UseCors(originsAllowed);
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints => endpoints.MapControllers());
app.Run();