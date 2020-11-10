using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Caulius.Extensions
{
	/// <summary>
	/// Extensions to emulate a typical "Startup.cs" pattern for <see cref="IHostBuilder"/>
	/// </summary>
	public static class HostBuilderExtensions
	{
		private const string ConfigureServicesMethodName = "ConfigureServices";

		/// <summary>
		/// Specify the startup type to be used by the host.
		/// </summary>
		/// <typeparam name="TStartup">The type containing an optional constructor with
		/// an <see cref="IConfiguration"/> parameter. The implementation should contain a public
		/// method named ConfigureServices with <see cref="IServiceCollection"/> parameter.</typeparam>
		/// <param name="hostBuilder">The <see cref="IHostBuilder"/> to initialize with TStartup.</param>
		/// <returns>The same instance of the <see cref="IHostBuilder"/> for chaining.</returns>
		public static IHostBuilder UseStartup<TStartup>(
			this IHostBuilder hostBuilder) where TStartup : class
		{
			// Invoke the ConfigureServices method on IHostBuilder...
			hostBuilder.ConfigureServices((hostContext, services) =>
			{
				// Find a method that has this signature: ConfigureServices(IServiceCollection)
				var configureServicesMethod = typeof(TStartup).GetMethod(
					ConfigureServicesMethodName, new[] { typeof(IServiceCollection) });

				// Check if TStartup has a constructor that takes a IConfiguration parameter
				var hasConfigConstructor = typeof(TStartup).GetConstructor(
					new[] { typeof(IConfiguration) }) != null;

				// Create a TStartup instance based on constructor
				var startupObject = hasConfigConstructor ?
					(TStartup)Activator.CreateInstance(typeof(TStartup), hostContext.Configuration) :
					(TStartup)Activator.CreateInstance(typeof(TStartup), null);

				// Finally, call the ConfigureServices implemented by the TStartup object
				configureServicesMethod?.Invoke(startupObject, new object[] { services });
			});

			// Chain the response
			return hostBuilder;
		}
	}
}