using Core.Base.Auth;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

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
