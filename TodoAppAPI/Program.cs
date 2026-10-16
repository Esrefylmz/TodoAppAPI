using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Text;
using System.Text.Json.Serialization;
using TodoAppAPI.ExceptionHandling;
using TodoAppAPI.Services;
using TodoAppApplication;
using TodoAppApplication.Abstractions;
using TodoAppApplication.Services;
using TodoAppCore.Models;
using TodoAppInfrastructure;




var builder = WebApplication.CreateBuilder(args);


// Application
builder.Services.AddApplication();

// Infrastructure
var connectionString =
    builder.Configuration.GetConnectionString("localdb")
    ?? throw new InvalidOperationException(
        "Connection string 'localdb' not found.");

builder.Services.AddInfrastructure(connectionString);


// API-specific services
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IPasswordService, PasswordService>();


// Password hashing
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();


// HttpContext
builder.Services.AddHttpContextAccessor();


// Controllers
builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters
            .Add(new JsonStringEnumConverter());
    });


// Global error handling
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();


// JWT Authentication
var jwtKey = builder.Configuration["Jwt:Key"];

if (string.IsNullOrWhiteSpace(jwtKey))
{
    throw new InvalidOperationException(
        "JWT signing key is not configured. Set 'Jwt:Key' using .NET User Secrets.");
}

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey)
            )
        };
    });

builder.Services.AddAuthorization();


// Swagger
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc(
        "v1",
        new OpenApiInfo
        {
            Title = "TodoAppAPI",
            Version = "v1"
        });

    options.AddSecurityDefinition(
        "bearer",
        new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            Description = "JWT Authorization header using the Bearer scheme."
        });

    options.AddSecurityRequirement(document =>
        new OpenApiSecurityRequirement
        {
            [new OpenApiSecuritySchemeReference("bearer", document)] = []
        });
});


var app = builder.Build();


// Error handling
app.UseExceptionHandler();
app.UseStatusCodePages();


// Swagger
app.UseSwagger();

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint(
        "/swagger/v1/swagger.json",
        "TodoAppAPI_V1");
});


// Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();


// Controllers
app.MapControllers();

app.Run();