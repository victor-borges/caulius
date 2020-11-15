using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;
using System;
using System.Linq;
using Caulius.Client.Handlers;
using Caulius.Client.Options;

namespace Caulius.Client
{
    public class CauliusService : BackgroundService
    {
        private readonly CauliusOptions _options;
        private readonly DiscordSocketClient _client;
        private readonly ILogger<CauliusService> _logger;
        private readonly CommandService _commands;
        private readonly IServiceProvider _services;

        public CauliusService(
            IOptions<CauliusOptions> options,
            DiscordSocketClient client,
            ILogger<CauliusService> logger,
            CommandService commands,
            IServiceProvider services)
        {
            _options = options.Value;
            _client = client;
            _logger = logger;
            _commands = commands;
            _services = services;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.WhenAll(
                AddLogHandlersAsync(),
                AddMessageHandlersAsync(),
                _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services));

            _client.Ready += SetGameAsync;

            await _client.LoginAsync(TokenType.Bot, _options.Token);
            await _client.StartAsync();

            await Task.Delay(-1, stoppingToken);
        }

        private Task AddLogHandlersAsync()
        {
            _client.Log += Log;
            _commands.Log += Log;

            return Task.CompletedTask;
        }

        private async Task AddMessageHandlersAsync()
        {
            var messageHandlers =
                from type in Assembly.GetExecutingAssembly().GetTypes()
                where type.GetInterfaces().Contains(typeof(IMessageHandler))
                select _services.GetService(type) as IMessageHandler;

            foreach (var handler in messageHandlers)
                await handler.SetupHandlerAsync();
        }

        private Task SetGameAsync() =>
            _client.SetGameAsync($"{_options.Prefix}help", type: ActivityType.Watching);

        private Task Log(LogMessage message)
        {
            var logLevel = message.Severity switch
            {
                LogSeverity.Critical => LogLevel.Critical,
                LogSeverity.Error => LogLevel.Error,
                LogSeverity.Warning => LogLevel.Warning,
                LogSeverity.Info => LogLevel.Information,
                LogSeverity.Verbose => LogLevel.Debug,
                LogSeverity.Debug => LogLevel.Trace,
                _ => LogLevel.Information
            };

            _logger.Log(logLevel, message.ToString(prependTimestamp: false));

            return Task.CompletedTask;
        }
    }
}
