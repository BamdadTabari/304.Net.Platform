﻿using Core.Base.Auth;
using Core.Base.EF;
using DataLayer.Repository;
using Hangfire;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

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

// JWT Config
var jwtSection = builder.Configuration.GetSection("JWT");
var jwtConfig = jwtSection.Get<SecurityTokenConfig>();
builder.Services.AddSingleton(jwtConfig);

builder.Services.AddControllers(options =>
{
	options.RespectBrowserAcceptHeader = true; // به Accept header احترام بگذار
	options.ReturnHttpNotAcceptable = true;    // اگر Accept پشتیبانی نشد، 406 برگردون
})
	.AddJsonOptions(options =>
	{
		options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
		options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
		options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
		options.JsonSerializerOptions.WriteIndented = true; // فقط برای خوانایی بهتر
	});
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddControllers();
// Database connection
string connectionString = builder.Environment.IsDevelopment()
	? builder.Configuration.GetConnectionString("ServerDbConnection")
	: builder.Configuration.GetConnectionString("ProductionDbConnection");


builder.Services.AddDbContext<ApplicationDbContext>(options =>
	options.UseSqlServer(connectionString).EnableDetailedErrors());

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
	{
		Title = "304 .Net",
		Version = "v1"
	});

	c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		Name = "Authorization",
		Type = SecuritySchemeType.ApiKey,
		Scheme = "Bearer",
		BearerFormat = "JWT",
		In = ParameterLocation.Header,
		Description = "Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOi...\""
	});

	c.AddSecurityRequirement(new OpenApiSecurityRequirement
	{
		{
			new OpenApiSecurityScheme
			{
				Reference = new Microsoft.OpenApi.Models.OpenApiReference
				{
					Type = ReferenceType.SecurityScheme,
					Id = "Bearer"
				}
			},
			Array.Empty<string>()
		}
	});
});

builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowSpecific",
		policy =>
		{
			policy
				  .AllowAnyOrigin()
				  .AllowAnyHeader()
				  .AllowAnyMethod();
		});
});
// In middleware:

// Repositories and services
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromMinutes(30);
	options.Cookie.HttpOnly = true;
	options.Cookie.IsEssential = true;
});

// JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options =>
	{
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidateLifetime = true,
			ValidateIssuerSigningKey = true,
			ValidIssuer = jwtConfig.Issuer,
			ValidAudience = jwtConfig.Audience,
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Key)),
			NameClaimType = ClaimTypes.NameIdentifier // این خط را اضافه کنید تا `NameIdentifier` به درستی ست شود
		};

		options.Events = new JwtBearerEvents
		{
			OnMessageReceived = context =>
			{
				var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
				if (string.IsNullOrEmpty(token))
				{
					token = context.Request.Cookies["jwt"]; // اگر توکن در هدر نبود از کوکی بخون
				}

				if (!string.IsNullOrEmpty(token))
				{
					context.Token = token;
				}

				return Task.CompletedTask;
			}
		};
	});


// Hangfire
builder.Services.AddHangfire(config => config.UseSqlServerStorage(connectionString));
builder.Services.AddHangfireServer();
builder.Services.AddSignalR();

var app = builder.Build();

// Middlewares
app.UseCors("AllowSpecific");
//app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
	FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "images")),
	RequestPath = "/images"
});
app.UseStaticFiles(new StaticFileOptions
{
	FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "images")),
	RequestPath = "/logs"
});
// Middlewareها
app.UseSerilogRequestLogging(); // middleware لاگ‌کردن درخواست‌ها

app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
	app.UseDeveloperExceptionPage();
	app.UseSwagger();
	app.UseSwaggerUI(options =>
	{
		options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
		//options.InjectJavascript("/swagger/swagger-authtoken.js");
	});
}

app.MapControllers();

app.Run();
