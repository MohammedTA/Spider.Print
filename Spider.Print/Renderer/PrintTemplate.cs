using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Spider.Print.Renderer
{
	public abstract class PrintTemplate<T> : IPrintTemplate<T>
	{
		private readonly MySettingsConfig _mySettingsConfig;
		private readonly IViewRenderService _viewRenderService;
		protected readonly string TemplateName;

		protected PrintTemplate(IOptions<MySettingsConfig> appConfig, IViewRenderService viewRenderService, string name)
		{
			_mySettingsConfig = appConfig.Value;
			_viewRenderService = viewRenderService;
			TemplateName = name;
		}

		public async Task<string> Compile(T model)
		{
			return await _viewRenderService.RenderToString(model);
		}

		protected abstract string GetSubject(T model);
	}
}