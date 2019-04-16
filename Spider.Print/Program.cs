using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Spider.Print.Models;
using Spider.Print.Renderer;

namespace Spider.Print
{
	internal class Program
	{
		private static async Task Main(string[] args)
		{
			Console.WriteLine("WELCOME TO SPIDER PRINTER APP.");

			// create service collection
			var services = new ServiceCollection();
			ConfigureServices(services);

			// create service provider
			var serviceProvider = services.BuildServiceProvider();


			// entry to run app
			await serviceProvider.GetService<App>().Run();
		}

		private static void ConfigureServices(IServiceCollection services)
		{
			// build config
			var configuration = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", false)
				.AddEnvironmentVariables()
				.Build();


			services.AddOptions();
			services.Configure<MySettingsConfig>(configuration.GetSection("MySettings"));


			// add app
			services.AddSingleton<IPrinterSender, PrinterSender>();
			services.AddSingleton(t => new DependencyInjectionContainer(t.GetService));
			services.AddSingleton<PrintTemplateRegister>();
			services.AddSingleton<IViewRenderService, ViewRenderService>();

			services.AddTransient<App>();
		}
	}
}