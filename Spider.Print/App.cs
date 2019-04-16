using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Spider.Print.Models;
using Spider.Print.Renderer;

namespace Spider.Print
{
	public class App
	{
		private readonly MySettingsConfig _mySettingsConfig;
		private readonly PrintTemplateRegister _printerSender;

		public App(IOptions<MySettingsConfig> mySettingsConfig, PrintTemplateRegister printerSender)
		{
			this._printerSender = printerSender;
			this._mySettingsConfig =
				mySettingsConfig?.Value ?? throw new ArgumentNullException(nameof(mySettingsConfig));
		}

		public async Task Run()
		{
			AdminOrder order;

			if (this._mySettingsConfig.TestingMode)
			{
				Console.WriteLine($"\n Get Order (x9YvekEiSYM5eAUwyP7Q) details our api");
				order = await GetOrder("x9YvekEiSYM5eAUwyP7Q");
			}
			else
			{
				Console.Write("Enter AdminOrder id: ");
				var orderId = Console.ReadLine();
				Console.WriteLine($"\n Get Order ({orderId}) details our api");
				order = await GetOrder(orderId);
			}

			if (order == null)
			{
				Console.WriteLine("\n Order retrieving process failed");
				Console.WriteLine("\n Press any key to close the application....");
				Console.ReadLine();
				return;
			}
			// printing service
			_printerSender.SendToPrinter(
				order, $"Order {order.Id}").Wait();
		}

		/// <summary>
		///     Get admin order details
		/// </summary>
		/// <param name="id">Admin id</param>
		public async Task<AdminOrder> GetOrder(string id)
		{
			using (var client = new WebClient())
			{
				var uri = new Uri(_mySettingsConfig.ApiUrl);
				client.Headers.Add("Content-Type:application/json");
				client.Headers.Add("Accept:application/json");

				var data = await client.DownloadStringTaskAsync($"{uri}/customer-orders/{id}");

				var response = JsonConvert.DeserializeObject<List<Response>>(data)[0];

				var adminOrder = JsonConvert.DeserializeObject<AdminOrder>(response.Content);

				Console.WriteLine($"\n Order ({adminOrder.Id}) details retrieved from the api");

				return adminOrder;
			}
		}
	}

	public class Response
	{
		public int Id { get; set; }
		public string Content { get; set; }
		public DateTime ModifiedDate { get; set; }
		public string TaskReference { get; set; }
	}
}