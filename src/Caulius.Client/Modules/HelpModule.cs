using Discord;
using Discord.Commands;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Caulius.Client.Modules
{
    [UsedImplicitly]
    public class HelpModule : ModuleBase<SocketCommandContext>
    {
        [UsedImplicitly]
        public CommandService CommandService { get; set; } = null!;

        [Command("help")]
        public Task GetHelpAsync()
        {
            var builder = new StringBuilder();

            foreach (var command in CommandService.Commands)
            {
                builder.AppendLine($"**`!{command.Name}`**");
            }

            var embed = new EmbedBuilder()
                .WithColor(Color.LighterGrey)
                .WithTitle("Comandos do Caulius")
                .WithDescription(builder.ToString())
                .Build();

            return ReplyAsync(embed: embed);
        }
    }
}
