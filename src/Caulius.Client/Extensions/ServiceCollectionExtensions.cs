using System.Reflection;
using Caulius.Client.Handlers;
using Caulius.Client.Options;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Caulius.Client.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureCauliusOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<CauliusOptions>(options =>
            configuration.Bind(nameof(CauliusOptions), options));

        return services;
    }

    public static IServiceCollection AddMessageHandlers(this IServiceCollection services)
    {
        return services.AddImplementingTypes<IMessageHandler>();
    }

    private static IServiceCollection AddImplementingTypes<T>(this IServiceCollection services)
    {
        foreach (var type in from type in Assembly.GetExecutingAssembly().GetTypes()
                 where type.GetInterfaces().Contains(typeof(T))
                 select type)
        {
            services.AddSingleton(type);
        }

        return services;
    }

    public static IServiceCollection AddDiscordClient(this IServiceCollection services)
    {
        services.AddSingleton(provider => new DiscordSocketClient(new DiscordSocketConfig
        {
            LogLevel = provider.GetService<IHostEnvironment>().IsProduction() ? LogSeverity.Info : LogSeverity.Debug
        }));

        return services;
    }

    public static IServiceCollection AddCommandService(this IServiceCollection services)
    {
        services.AddSingleton(provider => new CommandService(new CommandServiceConfig
        {
            LogLevel = provider.GetService<IHostEnvironment>().IsProduction() ? LogSeverity.Info : LogSeverity.Debug,
            CaseSensitiveCommands = false,
            IgnoreExtraArgs = true
        }));

        return services;
    }
}