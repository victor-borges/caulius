using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.Threading.Tasks;
using Caulius.Configuration;
using Caulius.Extensions;
using System.Reflection;

namespace Caulius
{
    public static class Program
    {
        private static async Task Main(string[] args)
        { 
            await CreateHostBuilder(args).Build().RunAsync();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, configurationBuilder) =>
                {
                    if (hostingContext.HostingEnvironment.IsDevelopment())
                        configurationBuilder.AddUserSecrets(Assembly.GetAssembly(typeof(Program)));

                    configurationBuilder.AddEnvironmentVariables(CauliusSettings.SettingsPrefix);
                })
                .UseSerilog((hostingContext, loggerConfiguration) =>
                {
                    loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration);
                })
                .UseStartup<Startup>();
    }
}
