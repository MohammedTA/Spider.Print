using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Spider.Print.Models;
using Spider.Print.Templates;

namespace Spider.Print.Renderer
{
	public class PrintTemplateRegister
	{
		private readonly DependencyInjectionContainer _dependencyInjectionContainer;
		private readonly ConcurrentDictionary<Type, Type> _printTemplateTypes = new ConcurrentDictionary<Type, Type>();
		private readonly IPrinterSender _printerSender;
		private readonly IOptions<MySettingsConfig> _mySettingsConfig;


		public PrintTemplateRegister(IPrinterSender printerSender,
			DependencyInjectionContainer dependencyInjectionContainer, IOptions<MySettingsConfig> mySettingsConfig)
		{
			_printerSender = printerSender;
			_dependencyInjectionContainer = dependencyInjectionContainer;
			_mySettingsConfig = mySettingsConfig;
		}

		public async Task<string> CompilePrint<T>(T model)
		{

			var instance = new OrderTemplate(this._mySettingsConfig, new ViewRenderService());
			var template = (IPrintTemplate<T>) instance;
			return await template.Compile(model);
		}

		public void RegisterAssembly(Assembly assembly)
		{
			assembly.ExportedTypes
				.Where(t => t.ImplementsGenericType(typeof(PrintTemplate<>)))
				.ForEach(t =>
				{
					var baseclass = t.GetBaseClassOfType(typeof(PrintTemplate<>));
					var modelType = baseclass.GenericTypeArguments[0];
					_printTemplateTypes.TryAdd(modelType, t);
				});
		}

		public async Task SendToPrinter<T>(T model, string subject)
		{
			var body = await CompilePrint(model);
			_printerSender.Sender(body, subject);
		}
	}
}