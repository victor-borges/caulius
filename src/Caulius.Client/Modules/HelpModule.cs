using Discord;
using Discord.Commands;
using Caulius.Client.Options;
using Microsoft.Extensions.Options;

namespace Caulius.Client.Modules;

public class HelpModule : ModuleBase<SocketCommandContext>
{
    public CommandService CommandService { get; set; } = null!;

    public IOptions<CauliusOptions> Options { get; set; } = null!;

    [Command("help")]
    [Summary("Displays this help message.")]
    public Task GetHelpAsync()
    {
        var fields = CommandService.Commands
            .Select(command =>
                new EmbedFieldBuilder()
                    .WithName($"{Options.Value.CommandPrefix}{command.Name}")
                    .WithValue(command.Summary))
            .ToList();

        var embed = new EmbedBuilder()
            .WithColor(Color.LighterGrey)
            .WithTitle("Caulius Help | Commands")
            .WithFields(fields)
            .Build();

        return ReplyAsync(embed: embed);
    }
}