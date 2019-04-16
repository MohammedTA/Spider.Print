using System.Threading.Tasks;

namespace Spider.Print.Renderer
{
	public interface IPrintTemplate<in TModel>
	{
		Task<string> Compile(TModel model);
	}
}