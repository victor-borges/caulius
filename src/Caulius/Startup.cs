using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Caulius.Configuration;
using Caulius.Domain.Services;

namespace Caulius
{
    public class Startup
    {
		public IConfiguration Configuration { get; }

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
		{
			services.Configure<CauliusSettings>(options => Configuration.Bind("CauliusSettings", options));

			services.AddSingleton(serviceProvider => new DiscordSocketClient(new DiscordSocketConfig
			{
				LogLevel = LogSeverity.Debug
			}));

			services.AddSingleton(serviceProvider => new CommandService(new CommandServiceConfig
			{
				LogLevel = LogSeverity.Debug,
				CaseSensitiveCommands = false,
				IgnoreExtraArgs = true
			}));

			services.AddSingleton<CommandHandler>();

			services.AddHostedService<CauliusService>();
		}
	}
}
