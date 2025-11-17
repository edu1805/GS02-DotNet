using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WellWork.Domain.Interfaces;
using WellWork.Infrastructure.Persistence;

namespace WellWork.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connString = configuration.GetConnectionString("OracleConnection");

        services.AddDbContext<WellWorkDbContext>(options =>
        {
            options.UseOracle(connString, opts =>
            {
                opts.MigrationsAssembly(typeof(WellWorkDbContext).Assembly.FullName);
            });
        });

        // Repositórios da aplicação
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ICheckInRepository, CheckInRepository>();
        services.AddScoped<IGeneratedMessageRepository, GeneratedMessageRepository>();

        return services;
    }
}