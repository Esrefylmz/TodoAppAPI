using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Text;
using System.Text.Json.Serialization;
using TodoAppAPI.DTOs.Todo;
using TodoAppAPI.ExceptionHandling;
using TodoAppAPI.MappingProfiles;
using TodoAppAPI.Services;
using TodoAppAPI.Validators.Todo;
using TodoAppCore.Models;
using TodoAppCore.Repositories;
using TodoAppInfrastructure.Data;
using TodoAppInfrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);


// Repositories
builder.Services.AddScoped<ITodoRepository, TodoRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();


// Services
builder.Services.AddScoped<ITodoService, TodoService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();


// Password hashing
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();


// FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<CreateTodoDtoValidator>();


builder.Services.AddHttpContextAccessor();


// AutoMapper
builder.Services.AddAutoMapper(
    cfg =>
    {
        // cfg.LicenseKey = "...";
    },
    typeof(TodoProfile).Assembly,
    typeof(CategoryProfile).Assembly
);


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


// Database
var connectionString =
    builder.Configuration.GetConnectionString("localdb")
    ?? throw new InvalidOperationException(
        "Connection string 'localdb' not found.");

builder.Services.AddDbContext<TodoContext>(options =>
    options.UseSqlServer(connectionString));


var app = builder.Build();


// Error handling
app.UseExceptionHandler();
app.UseStatusCodePages();


// Swagger
app.UseSwagger();

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json",
        "TodoAppAPI_V1");
});


// Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();


// Controllers
app.MapControllers();

app.Run();