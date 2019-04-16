using System.Reflection;
using System.Threading.Tasks;
using RazorLight;

namespace Spider.Print.Renderer
{
	/// <summary>
	///     Razor template engine.
	/// </summary>
	public class ViewRenderService : IViewRenderService
	{
		private readonly RazorLightEngine engine = new RazorLightEngineBuilder()
			.UseMemoryCachingProvider()
			.Build();

		public async Task<string> RenderToString<TModel>(TModel model)
		{
			var type = model.GetType();
			//var typeDeclaringType = type.DeclaringType;

			//if (typeDeclaringType == null) throw new Exception("Invalid");

			var templateBody = await RenderPartialView(
				type.Assembly,
				"Spider.Print.Templates.OrderTemplate.cshtml",
				model);

			return templateBody;
		}

		/// <summary>
		///     Renders "cshtml" email template to string.
		/// </summary>
		/// <param name="assembly">Assembly where the embedded resource resides.</param>
		/// <param name="embeddedResourceName"></param>
		/// <param name="model">The model to pass to the view.</param>
		private async Task<string> RenderPartialView<T>(Assembly assembly, string embeddedResourceName, T model)
		{
			var modelType = typeof(T);

			var cache = engine.TemplateCache.RetrieveTemplate(modelType.FullName);

			if (!cache.Success)
			{
				var source = assembly.GetEmbeddedResourceText(embeddedResourceName);
				return await engine.CompileRenderAsync(modelType.FullName, source, model);
			}

			return await engine.RenderTemplateAsync(cache.Template.TemplatePageFactory(), model);
		}
	}
}