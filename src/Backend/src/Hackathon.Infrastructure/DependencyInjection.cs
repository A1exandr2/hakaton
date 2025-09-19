using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Hackathon.Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Hackathon.Infrastructure.Data;
using Hackathon.Domain.Repositories;
using Hackathon.Infrastructure.Repositories;

namespace Hackathon.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var connectionString = configuration.GetConnectionString("Postgresql");

        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString)
        );

        services.AddSingleton<ClickHouseInitializer>();

        services.AddTransient<IUserRepository, UsersRepository>();
        services.AddTransient<IServersRepository, ServersRepository>();
        services.AddTransient<IPingLogsRepository, PingLogsRepository>();

        return services;
    }

    public static async Task AddClickHouse(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var initializer = scope.ServiceProvider.GetRequiredService<ClickHouseInitializer>();
        try
        {
            await initializer.InitializeAsync();
        }
        catch
        {
            throw;
        }
    }
}