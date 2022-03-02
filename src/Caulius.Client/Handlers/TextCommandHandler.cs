using Caulius.Client.Options;
using Caulius.Infrastructure;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;

namespace Caulius.Client.Handlers;

public class TextCommandHandler : IMessageHandler
{
    private readonly DiscordSocketClient _client;
    private readonly CauliusOptions _options;
    private readonly CauliusContext _context;

    public TextCommandHandler(DiscordSocketClient client, IOptions<CauliusOptions> options, CauliusContext context)
    {
        _client = client;
        _options = options.Value;
        _context = context;
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

        if (message.Author.IsBot || !message.HasStringPrefix(_options.TextCommandPrefix, ref argPos))
            return;

        var command = socketMessage.Content[argPos..].Split(' ')[0];

        if (string.IsNullOrEmpty(command))
            return;

        var textCommand = await _context.TextCommands
            .SingleOrDefaultAsync(t => t.Command == command);

        if (textCommand is not null)
            await socketMessage.Channel.SendMessageAsync(textCommand.Text);
    }
}
