using System;

namespace Spider.Print.Models
{
	public class AdminOrder
	{
		public AdminOrder()
		{
		}
		public string Id { get; set; }
		public string CustomerName { get; set; }
		public string CustomerEmail { get; set; }
		public string CustomerPhone { get; set; }
		public string CustomerAddress { get; set; }
		public string RecieptNumber { get; set; }
		public string CustomerState { get; set; }
		public string Status { get; set; }
		public string Price { get; set; }
		public decimal DeliveryCharge { get; set; }
		public decimal CashCollectAmount { get; set; }
		public decimal PriceInInvoice { get; set; }

		public DateTime CreatedDate { get; set; }

		//  public string DeliveryDate { get; set; }
		// public string DeliveryTime { get; set; }
		public DateTime? CompletedDate { get; set; }
		public string AgentName { get; set; }
		public AdminStore Store { get; set; }
	}
}