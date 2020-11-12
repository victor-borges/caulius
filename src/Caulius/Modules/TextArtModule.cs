using Caulius.Data.Entities;
using Caulius.Data.Repositories.Abstractions;
using Discord.Commands;
using Discord.Commands.Builders;

namespace Caulius.Client.Modules
{
    public class TextArtModule : ModuleBase<SocketCommandContext>
    {
        private readonly IReadOnlyRepository<TextArtCommand> _commandRepository;

        public TextArtModule(IReadOnlyRepository<TextArtCommand> commandRepository)
        {
            _commandRepository = commandRepository;
        }

        protected override void OnModuleBuilding(CommandService commandService, ModuleBuilder builder)
        {
            foreach (var command in _commandRepository.FindAll())
            {
                builder.AddCommand(
                    command.Command,
                    async (context, args, services, commandInfo) =>
                    {
                        await context.Channel.SendMessageAsync(command.TextArt);
                    },
                    commandBuilder =>
                    {
                        commandBuilder.Summary = command.Summary;
                        commandBuilder.IgnoreExtraArgs = true;
                    });
            }

            base.OnModuleBuilding(commandService, builder);
        }
    }
}
