using System.Threading.Tasks;

namespace Spider.Print.Renderer
{
	public interface IViewRenderService
	{
		Task<string> RenderToString<TModel>(TModel model);
	}
}