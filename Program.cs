using client.EnergyMeter;
using client;
using Serilog;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Hackaton_2023
{
	internal class Program
	{
		static async Task Main(string[] args)
		{
		

			Log.Logger = new LoggerConfiguration()
				.WriteTo.Console()
				.CreateBootstrapLogger();

			await using var host = CreateHostBuilder(args).Build();

			await host.RunAsync();
			return;
		}

		static WebApplicationBuilder CreateHostBuilder(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);
			builder.Configuration.AddCommandLine(args);
			builder.Configuration.AddEnvironmentVariables();
			builder.Configuration.SetBasePath(Directory.GetCurrentDirectory());
			builder.Configuration.AddJsonFile("appsettings.json", false, true);
			builder.Configuration.AddEnvironmentVariables();
			builder.Configuration.AddUserSecrets<Program>();
			builder
				.Host
				.ConfigureServices((_, services) => { ConfigureServices(services); });

			return builder;
		}

		static void ConfigureServices(IServiceCollection services)
		{
			// MQTT
			services.AddHostedService<MqttClient>();
			services.AddSingleton<IMqttMessageHandler, MqttMessageHandler>();
			services.AddTransient<IEnergyReadingHandler, EnergyReadingHandler>();
		}
	}
}