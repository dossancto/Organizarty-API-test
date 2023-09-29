using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Organizarty.Infra.Data.Contexts;

namespace Organizarty.Infra.Extensions;

public static class InfraExtention
{

    private static string GetConnectionString(IConfiguration configuration)
    {
        string? envConnectionString = Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING");

        if (envConnectionString is not null)
        {
            Console.WriteLine("Using Environment database");
            return envConnectionString;
        }

        // TODO: Try change "Console.WriteLine()" to Logger.
        Console.WriteLine("Using development database");

        return configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connect string not found");
    }

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
          options.UseMySql(GetConnectionString(configuration),
          new MySqlServerVersion(new Version(8, 0, 26)),
          b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

        return services;
    }
}
