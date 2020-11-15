using Microsoft.Extensions.Hosting;
using Serilog;
using System.Threading.Tasks;
using Caulius.Client.Extensions;
using Caulius.Infrastructure.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Caulius.Client
{
    public static class Program
    {
        private static async Task Main(string[] args)
        {
            await CreateHostBuilder(args).Build().RunAsync();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog((hostingContext, loggerConfiguration) =>
                {
                    loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration);
                })
                .ConfigureServices((hostingContext, services) =>
                {
                    services
                        .ConfigureCauliusOptions(hostingContext.Configuration)
                        .AddDiscordClient()
                        .AddCommandService()
                        .AddHostedService<CauliusService>()
                        .AddCauliusContext(hostingContext.Configuration)
                        .AddRepositories()
                        .AddMessageHandlers();
                });
    }
}
