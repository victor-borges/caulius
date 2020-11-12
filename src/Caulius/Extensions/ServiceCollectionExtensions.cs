using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using Caulius.Data.Entities;
using Caulius.Data.Repositories;
using Caulius.Data.Repositories.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Caulius.Client.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddImplementingTypes<T>(this IServiceCollection services)
        {
            foreach (var type in from type in Assembly.GetExecutingAssembly().GetTypes()
                                 where type.GetInterfaces().Contains(typeof(T))
                                 select type)
            {
                services.AddSingleton(type);
            }
        }

        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddSingleton<IReadOnlyRepository<TextArtCommand>>(provider =>
            {
                var filePath = string.Format(CultureInfo.InvariantCulture,
                    "{0}{1}Assets{1}textart.json",
                    Directory.GetCurrentDirectory(),
                    Path.DirectorySeparatorChar);

                return new JsonFileRepository<TextArtCommand>(filePath);
            });
        }
    }
}
