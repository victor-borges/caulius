using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Caulius.Client.Extensions;
using Caulius.Client.Configuration;
using Caulius.Client.Handlers;

namespace Caulius.Client
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IHostEnvironment _hostEnvironment;

        public Startup(IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            _configuration = configuration;
            _hostEnvironment = hostEnvironment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CauliusOptions>(options =>
                _configuration.Bind("CauliusOptions", options));

            services.AddSingleton(serviceProvider => new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = _hostEnvironment.IsProduction() ? LogSeverity.Info : LogSeverity.Debug
            }));

            services.AddSingleton(serviceProvider => new CommandService(new CommandServiceConfig
            {
                LogLevel = _hostEnvironment.IsProduction() ? LogSeverity.Info : LogSeverity.Debug,
                CaseSensitiveCommands = false,
                IgnoreExtraArgs = true
            }));

            services.AddHostedService<CauliusService>();

            services.AddRepositories();

            services.AddImplementingTypes<IMessageHandler>();
        }
    }
}
