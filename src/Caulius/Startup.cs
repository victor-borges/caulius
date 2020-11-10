using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Caulius.Configuration;
using Caulius.Domain.Services;
using Microsoft.Extensions.Hosting;

namespace Caulius
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
			services.Configure<CauliusSettings>(options =>
                _configuration.Bind("CauliusSettings", options));

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

			services.AddSingleton<CommandHandler>();

			services.AddHostedService<CauliusService>();
		}
	}
}
