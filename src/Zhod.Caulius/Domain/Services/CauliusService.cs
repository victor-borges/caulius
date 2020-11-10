using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading;
using System.Threading.Tasks;
using Zhod.Caulius.Configuration;
using Zhod.Caulius.Domain.Modules;

namespace Zhod.Caulius.Domain.Services
{
    class CauliusService : BackgroundService
    {
        private readonly CauliusSettings _options;
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private readonly ILogger<CauliusService> _logger;
        private readonly CommandHandler _commandHandler;

        public CauliusService(IOptions<CauliusSettings> options, DiscordSocketClient client, CommandService commands, ILogger<CauliusService> logger, CommandHandler commandHandler)
        {
            _options = options.Value;
            _client = client;
            _commands = commands;
            _logger = logger;
            _commandHandler = commandHandler;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            InstallDelegates();
            await _commandHandler.InstallCommandsAsync();

            await _client.LoginAsync(TokenType.Bot, _options.BotToken);
            await _client.StartAsync();

            await Task.Delay(-1, stoppingToken);
        }

        private void InstallDelegates()
        {
            _client.Log += Log;
            _client.Ready += SetGameAsync;

            _commands.Log += Log;
        }

        private Task SetGameAsync() => _client.SetGameAsync($"{_options.Prefix}help", type: ActivityType.Listening);

        private Task Log(LogMessage message)
        {
            var logLevel = message.Severity switch
            {
                LogSeverity.Critical => LogLevel.Critical,
                LogSeverity.Error => LogLevel.Error,
                LogSeverity.Warning => LogLevel.Warning,
                LogSeverity.Info => LogLevel.Information,
                LogSeverity.Verbose => LogLevel.Debug,     // Verbose maps to Debug in the amount of details provided
                LogSeverity.Debug => LogLevel.Trace,       // Same here
                _ => LogLevel.Trace
            };

            _logger.Log(logLevel, message.ToString(prependTimestamp: false));
            return Task.CompletedTask;
        }
    }
}
