using Core.Base.Auth;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day) // فایل روزانه
    .CreateLogger();
//builder.Host.UseSerilog();
// configs
builder.Services.Configure<LockoutConfig>(
    builder.Configuration.GetSection(LockoutConfig.SectionName));
builder.Services.Configure<SecurityTokenConfig>(
    builder.Configuration.GetSection(SecurityTokenConfig.SectionName));

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
