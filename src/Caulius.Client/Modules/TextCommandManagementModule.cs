using System;
using System.Linq;
using Discord;
using Discord.Commands;
using System.Text;
using System.Threading.Tasks;
using Caulius.Client.Options;
using Caulius.Infrastructure;
using Microsoft.Extensions.Options;

namespace Caulius.Client.Modules
{
    public class TextCommandManagementModule : ModuleBase<SocketCommandContext>
    {
        private const int PageSize = 20;

        public CauliusContext CauliusContext { get; set; } = null!;

        public IOptions<CauliusOptions> Options { get; set; } = null!;

        [Command("commands")]
        [Summary("Displays a list of available commands.")]
        public async Task ListTextCommands(int page = 1)
        {
            if (page < 1)
                page = 1;

            var pageIndex = PageSize * (page - 1);
            var builder = new StringBuilder();

            await foreach (var command in CauliusContext.TextCommands.AsAsyncEnumerable().Skip(pageIndex).Take(PageSize))
            {
                builder.AppendLine($"**`{Options.Value.TextCommandPrefix}{command.Command}`**");
            }

            var count = await CauliusContext.TextCommands.CountAsync();
            var footer = $"Showing {pageIndex + 1}-{Math.Min(PageSize * page, count)} of {count} commands.";

            var embed = new EmbedBuilder()
                .WithColor(Color.LighterGrey)
                .WithTitle("Available commands:")
                .WithDescription(builder.ToString())
                .WithFooter(f => f.Text = footer)
                .Build();

            await ReplyAsync(embed: embed);
        }
    }
}
