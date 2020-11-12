using Microsoft.Extensions.Hosting;
using Serilog;
using System.Threading.Tasks;
using Caulius.Client.Extensions;

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
                .UseStartup<Startup>();
    }
}
