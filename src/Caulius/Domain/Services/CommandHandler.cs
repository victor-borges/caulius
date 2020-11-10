using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Options;
using System;
using System.Reflection;
using System.Threading.Tasks;
using Caulius.Configuration;

namespace Caulius.Domain.Services
{
    public class CommandHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private readonly IServiceProvider _services;
        private readonly CauliusSettings _options;

        public CommandHandler(DiscordSocketClient client, CommandService commands, IServiceProvider services, IOptions<CauliusSettings> options)
        {
            _commands = commands;
            _client = client;
            _services = services;
            _options = options.Value;
        }

        public async Task InstallCommandsAsync()
        {
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);

            _client.MessageReceived += HandleCommandAsync;
        }

        private async Task HandleCommandAsync(SocketMessage socketMessage)
        {
            if (!(socketMessage is SocketUserMessage message)) return;

            var argPos = 0;

            if (!(message.HasStringPrefix(_options.Prefix, ref argPos) ||
                message.HasMentionPrefix(_client.CurrentUser, ref argPos)) ||
                message.Author.IsBot)
                return;

            var context = new SocketCommandContext(_client, message);

            var result = await _commands.ExecuteAsync(context, argPos, _services);

            if (!result.IsSuccess)
                await context.Channel.SendMessageAsync(result.ErrorReason);
        }
    }
}
