namespace Spider.Print.Renderer
{
	public interface IPrinterSender
	{
		void Sender(string body, string subject);
	}
}