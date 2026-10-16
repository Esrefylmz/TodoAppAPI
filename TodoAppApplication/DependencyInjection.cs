using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using TodoAppApplication.MappingProfiles;
using TodoAppApplication.Services;
using TodoAppApplication.Validators.Todo;

namespace TodoAppApplication;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        // Services
        services.AddScoped<ITodoService, TodoService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IAuthService, AuthService>();

        // FluentValidation
        services.AddValidatorsFromAssemblyContaining<CreateTodoDtoValidator>();

        // AutoMapper
        services.AddAutoMapper(
            cfg => { },
            typeof(TodoProfile).Assembly);

        return services;
    }
}