using Discord;
using Discord.Commands;
using System.Text;
using System.Threading.Tasks;

namespace Caulius.Client.Modules
{
    public class HelpModule : ModuleBase<SocketCommandContext>
    {
        private readonly CommandService _commands;

        public HelpModule(CommandService commands)
        {
            _commands = commands;
        }

        [Command("help")]
        [Summary("Exibe esta mensagem.")]
        public Task GetHelpAsync()
        {
            var builder = new StringBuilder();

            foreach (var command in _commands.Commands)
            {
                builder.AppendLine($"**`!{command.Name}`**");
                builder.AppendLine(string.IsNullOrEmpty(command.Summary) ? "Sem descrição." : command.Summary);
                builder.AppendLine();
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
