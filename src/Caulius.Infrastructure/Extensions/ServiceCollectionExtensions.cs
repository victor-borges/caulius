using Caulius.Infrastructure.Seed;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Caulius.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCauliusContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<CauliusContext>(options =>
            {
                options.UseCosmos(configuration["Cosmos:ConnectionString"], "Caulius");
            },
            ServiceLifetime.Transient,
            ServiceLifetime.Transient);

        services.AddTransient<CauliusSeed>();
        services.BuildServiceProvider().GetRequiredService<CauliusSeed>().Seed();

        return services;
    }
}