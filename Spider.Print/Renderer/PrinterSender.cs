using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using IronPdf;
using Microsoft.Extensions.Options;

namespace Spider.Print.Renderer
{
	public class PrinterSender : IPrinterSender
	{
		private readonly MySettingsConfig _mySettingsConfig;

		public PrinterSender(IOptions<MySettingsConfig> mySettingsConfig)
		{
			_mySettingsConfig = mySettingsConfig.Value;
		}

		public void Sender(string body, string subject)
		{
			var pdfFile = BuildPdf(body, subject);
			SendToPrinter(pdfFile.BinaryData);
		}

		public PdfDocument BuildPdf(string body, string subject)
		{
			Console.WriteLine("\n Start building pdf file..");

			var location = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

			var today = DateTime.UtcNow.ToString("dd.MM.yyyy");
			const string tempDirName = "Temp";
			var htmlToPdf = new HtmlToPdf(); // new instance of HtmlToPdf
			var pdf = htmlToPdf.RenderHtmlAsPdf(body);

			var tempDir = Path.Combine(location, tempDirName);
			tempDir.Ensure();

			// save resulting pdf into file
			var fileName = $"{subject}-{today}.pdf";
			pdf.SaveAs(Path.Combine(tempDir, fileName));

			Console.WriteLine(
				$"\n The pdf file ({fileName}) has been built successfully, file location: {tempDir}");

			return pdf;
		}

		private void SendToPrinter(byte[] fileBytes)
		{
			Console.WriteLine("\n Start Printing the file...");

			var clientSocket =
				new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp) {NoDelay = true};

			var ip = IPAddress.Parse(_mySettingsConfig.PrinterIP);
			var ipEndPoint = new IPEndPoint(ip, _mySettingsConfig.PrinterPort);
			try
			{
				clientSocket.Connect(ipEndPoint);
				clientSocket.Send(fileBytes);
				clientSocket.Close();

				Console.WriteLine("\n Printing process has been finished successfully");
				Console.WriteLine("\n Press any key to close the application....");
				Console.ReadLine();
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				clientSocket.Close();
				Console.WriteLine("\n Press any key to close the application....");
				Console.ReadLine();
			}

			//var info = new ProcessStartInfo
			//{
			//	FileName = "lpr",
			//	CreateNoWindow = true,
			//	WindowStyle = ProcessWindowStyle.Hidden,
			//	UseShellExecute = true,
			//	Arguments = " -P " + printerName + " " + Path.Combine(location, fileName)

			//};


			//var printProcess = new Process
			//{
			//	StartInfo = info
			//};

			//printProcess.Start();

			//var proc = new Process
			//{
			//	StartInfo =
			//	{
			//		FileName = "lpr",
			//		WorkingDirectory = baseDir,
			//		WindowStyle = ProcessWindowStyle.Hidden,
			//		Arguments = " -S " + printerIp + " -P RAW " + filePath,
			//		UseShellExecute = true
			//	}
			//};
			//proc.Start();

			////var proc = Process.Start($"LPR -S {printerIp} -P {printerName} -Og {filePath}");

			//proc.WaitForExit();
			//Thread.Sleep(3000);
			//if (proc.HasExited)
			//	proc.Kill();
		}
	}
}