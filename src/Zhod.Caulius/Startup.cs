using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Zhod.Caulius.Configuration;
using Zhod.Caulius.Domain.Modules;
using Zhod.Caulius.Domain.Services;

namespace Zhod.Caulius
{
    class Startup
    {
		public IConfiguration Configuration { get; }

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
		{
			services.Configure<CauliusSettings>(options => Configuration.Bind("CauliusSettings", options));

			services.AddSingleton(services => new DiscordSocketClient(new DiscordSocketConfig
			{
				LogLevel = LogSeverity.Debug
			}));

			services.AddSingleton(services => new CommandService(new CommandServiceConfig
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
