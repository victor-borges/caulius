using Discord;
using Discord.Commands;
using System.Text;
using System.Threading.Tasks;

namespace Caulius.Client.Modules
{
    public class HelpModule : ModuleBase<SocketCommandContext>
    {
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
                .WithTitle("Caulius Help | Commands")
                .WithDescription(builder.ToString())
                .Build();

            return ReplyAsync(embed: embed);
        }
    }
}
