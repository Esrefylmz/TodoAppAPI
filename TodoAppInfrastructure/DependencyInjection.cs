using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TodoAppCore.Repositories;
using TodoAppInfrastructure.Data;
using TodoAppInfrastructure.Repositories;

namespace TodoAppInfrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        string connectionString)
    {
        // Repositories
        services.AddScoped<ITodoRepository, TodoRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        // Database
        services.AddDbContext<TodoContext>(options =>
            options.UseSqlServer(connectionString));

        return services;
    }
}