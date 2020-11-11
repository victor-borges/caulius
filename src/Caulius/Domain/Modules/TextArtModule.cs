using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.Json;
using Caulius.Data;
using Discord.Commands;
using Discord.Commands.Builders;

namespace Caulius.Domain.Modules
{
    public class TextArtModule : ModuleBase<SocketCommandContext>
    {
        private readonly string _textArtFilePath =
            string.Format(CultureInfo.InvariantCulture, "{0}{1}Data{1}textart.json", Directory.GetCurrentDirectory(), Path.DirectorySeparatorChar);

        protected override void OnModuleBuilding(CommandService commandService, ModuleBuilder builder)
        {
            var fileContent = File.ReadAllText(_textArtFilePath);
            var commands = JsonSerializer.Deserialize<IEnumerable<TextArtCommand>>(fileContent);

            foreach (var command in commands)
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
