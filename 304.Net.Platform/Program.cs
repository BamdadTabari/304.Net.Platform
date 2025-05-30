using Core.Base.Auth;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// پیکربندی Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug() // یا .Information() یا .Error()
    .Enrich.FromLogContext()
    .Enrich.WithEnvironmentName()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog(); // اتصال Serilog به Host

// configs
builder.Services.Configure<LockoutConfig>(
    builder.Configuration.GetSection(LockoutConfig.SectionName));
builder.Services.Configure<SecurityTokenConfig>(
    builder.Configuration.GetSection(SecurityTokenConfig.SectionName));

var app = builder.Build();

// Middlewareها
app.UseSerilogRequestLogging(); // middleware لاگ‌کردن درخواست‌ها

app.UseRouting();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
