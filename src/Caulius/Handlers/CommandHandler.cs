using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Options;
using System;
using System.Reflection;
using System.Threading.Tasks;
using Caulius.Configuration;

namespace Caulius.Handlers
{
    public class CommandHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private readonly IServiceProvider _services;
        private readonly CauliusSettings _options;

        public CommandHandler(
            DiscordSocketClient client,
            CommandService commands,
            IServiceProvider services,
            IOptions<CauliusSettings> options)
        {
            _client = client;
            _commands = commands;
            _services = services;
            _options = options.Value;
        }

        public async Task InitializeAsync()
        {
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);

            _client.MessageReceived += HandleCommandAsync;
        }

        private async Task HandleCommandAsync(SocketMessage socketMessage)
        {
            if (socketMessage is not SocketUserMessage message)
                return;

            var argPos = 0;

            if (message.Author.IsBot || !message.HasStringPrefix(_options.Prefix, ref argPos))
                return;

            var context = new SocketCommandContext(_client, message);

            await _commands.ExecuteAsync(context, argPos, _services);
        }
    }
}
