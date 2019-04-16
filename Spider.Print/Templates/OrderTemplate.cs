using Microsoft.Extensions.Options;
using Spider.Print.Models;
using Spider.Print.Renderer;

namespace Spider.Print.Templates
{
	public class OrderTemplate : PrintTemplate<AdminOrder>
	{
		public OrderTemplate(IOptions<MySettingsConfig> appConfig, IViewRenderService viewRenderService)
			: base(appConfig, viewRenderService, nameof(OrderTemplate))
		{
		}

		protected override string GetSubject(AdminOrder model)
		{
			return $"Admin order #{model.Id}";
		}
	}
}