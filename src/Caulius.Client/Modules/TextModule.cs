using Caulius.Infrastructure;
using Discord.Commands;
using Discord.Commands.Builders;
using Microsoft.EntityFrameworkCore;

namespace Caulius.Client.Modules
{
    public class TextModule : ModuleBase<SocketCommandContext>
    {
        public CauliusContext CauliusContext { get; set; } = null!;

        protected override void OnModuleBuilding(CommandService commandService, ModuleBuilder builder)
        {
            foreach (var command in CauliusContext.TextCommands.AsNoTracking())
            {
                builder.AddCommand(
                    command.Command,
                    async (context, _, _, _) =>
                    {
                        await context.Channel.SendMessageAsync(command.Text);
                    },
                    commandBuilder =>
                    {
                        commandBuilder.IgnoreExtraArgs = true;
                    });
            }

            base.OnModuleBuilding(commandService, builder);
        }
    }
}
