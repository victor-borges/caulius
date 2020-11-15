using Caulius.Domain.Aggregates.TextArt;
using Caulius.Domain.Common;
using Discord.Commands;
using Discord.Commands.Builders;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace Caulius.Client.Modules
{
    [UsedImplicitly]
    public class TextModule : ModuleBase<SocketCommandContext>
    {
        [UsedImplicitly]
        public IRepository<TextCommand> TextCommandRepository { get; set; } = null!;

        protected override void OnModuleBuilding(CommandService commandService, ModuleBuilder builder)
        {
            foreach (var command in TextCommandRepository.Query.AsNoTracking())
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
