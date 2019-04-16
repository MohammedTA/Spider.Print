using System.IO;

namespace Spider.Print
{
	public static class Extension
	{
		public static void Ensure(this string tempDir)
		{
			if (!Directory.Exists(tempDir)) Directory.CreateDirectory(tempDir);
		}
	}
}