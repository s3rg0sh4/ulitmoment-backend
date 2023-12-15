using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;
using UlitMoment;
using UlitMoment.Configuration;
using UlitMoment.Database;
using UlitMoment.Features.Auth;
using UlitMoment.Middleware;

EnvLoader.Load(".env");

var builder = WebApplication.CreateBuilder(args);

// Application settings
var applicationSettings = new ApplicationSettings();
builder.Configuration.GetSection(ApplicationSettings.SectionName).Bind(applicationSettings);

// Utilities
builder
    .Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// Logging
builder
    .Host
    .UseSerilog(
        (context, configuration) =>
        {
            configuration
                .MinimumLevel
                .Information()
                .MinimumLevel
                .Override("Microsoft", LogEventLevel.Information)
                .MinimumLevel
                .Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                .MinimumLevel
                .Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                .MinimumLevel
                .Override("Microsoft.EntityFrameworkCore.Database", LogEventLevel.Warning)
                .MinimumLevel
                .Override("Microsoft.AspNetCore.Hosting.Diagnostics", LogEventLevel.Warning)
                .MinimumLevel
                .Override("Microsoft.AspNetCore.StaticFiles", LogEventLevel.Warning)
                .MinimumLevel
                .Override("Microsoft.EntityFrameworkCore.Query", LogEventLevel.Error)
                .Enrich
                .FromLogContext();

            if (context.HostingEnvironment.IsProduction())
                configuration.WriteTo.Console(new JsonFormatter());
            else
                configuration.WriteTo.Console();
        }
    );

// Auth
builder
    .Services
    .AddAuthorizationBuilder()
    .SetDefaultPolicy(
        new AuthorizationPolicyBuilder("Bearer")
            .RequireAuthenticatedUser()
            .RequireClaim("UserId")
            .Build()
    )
    .AddPolicy("RefreshToken", policy => policy.RequireAuthenticatedUser().RequireClaim("UserId"));

builder
    .Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(
        options =>
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(applicationSettings.JWT.AccessSecret)
                ),
                ValidateIssuer = true,
                ValidIssuer = applicationSettings.JWT.ValidIssuer,
                ValidateAudience = true,
                ValidAudience = applicationSettings.JWT.ValidAudience,
                ClockSkew = TimeSpan.FromSeconds(30)
            }
    );

// Database
builder
    .Services
    .AddIdentity<User, IdentityRole<Guid>>()
    .AddRoles<IdentityRole<Guid>>()
    .AddRoleManager<RoleManager<IdentityRole<Guid>>>()
    .AddEntityFrameworkStores<UserContext>();

builder
    .Services
    .AddDbContext<UserContext>(
        options => options.UseNpgsql(applicationSettings.DbConnectionString)
    );

// Services
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<AuthService>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder
    .Services
    .AddSwaggerGen(options =>
    {
        options.MapType<DateOnly>(() => new OpenApiSchema { Type = "string", Format = "date" });
        options.AddSecurityDefinition(
            "Bearer",
            new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "bearer"
            }
        );
        options.AddSecurityRequirement(
            new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            }
        );
    });

builder.Services.AddCors();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseHttpsRedirection();

app.UseSerilogRequestLogging();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

app.Run();
