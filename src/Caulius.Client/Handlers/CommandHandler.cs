using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Caulius.Client.Options;

namespace Caulius.Client.Handlers
{
    public class CommandHandler : IMessageHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private readonly IServiceProvider _services;
        private readonly CauliusOptions _options;

        public CommandHandler(
            DiscordSocketClient client,
            CommandService commands,
            IServiceProvider services,
            IOptions<CauliusOptions> options)
        {
            _client = client;
            _commands = commands;
            _services = services;
            _options = options.Value;
        }

        public Task SetupHandlerAsync()
        {
            _client.MessageReceived += OnMessageReceivedAsync;
            return Task.CompletedTask;
        }

        private async Task OnMessageReceivedAsync(SocketMessage socketMessage)
        {
            if (socketMessage is not SocketUserMessage message)
                return;

            var argPos = 0;

            if (message.Author.IsBot || !message.HasStringPrefix(_options.CommandPrefix, ref argPos))
                return;

            var context = new SocketCommandContext(_client, message);

            await _commands.ExecuteAsync(context, argPos, _services);
        }
    }
}
