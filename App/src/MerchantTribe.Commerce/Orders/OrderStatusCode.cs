using System;
using System.Data;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace MerchantTribe.Commerce.Orders
{
	public class OrderStatusCode
	{
        public const string Cancelled = "A7FFDB90-C566-4cf2-93F4-D42367F359D5";
        public const string OnHold = "88B5B4BE-CA7B-41a9-9242-D96ED3CA3135";
        public const string Received = "F37EC405-1EC6-4a91-9AC4-6836215FBBBC";
        public const string ReadyForPayment = "e42f8c28-9078-47d6-89f8-032c9a6e1cce";
        public const string ReadyForShipping = "0c6d4b57-3e46-4c20-9361-6b0e5827db5a";
        public const string Completed = "09D7305D-BD95-48d2-A025-16ADC827582A";

        public string Bvin { get; set; }
        public DateTime LastUpdated { get; set; }	
		public string StatusName {get;set;}
		public bool SystemCode {get;set;}
		public int SortOrder {get;set;}

		public OrderStatusCode()
		{
            this.Bvin = string.Empty;
            this.LastUpdated = DateTime.UtcNow;
            this.StatusName = string.Empty;
            this.SystemCode = false;
            this.SortOrder = 0;
		}
	
        public static List<OrderStatusCode> FindAll()
        {
            List<OrderStatusCode> result = new List<OrderStatusCode>();

            result.Add(new OrderStatusCode() { Bvin = "A7FFDB90-C566-4cf2-93F4-D42367F359D5", SystemCode = true, StatusName = "Cancelled", SortOrder = 0 });
            result.Add(new OrderStatusCode() { Bvin = "88B5B4BE-CA7B-41a9-9242-D96ED3CA3135", SystemCode = true, StatusName = "On Hold", SortOrder = 1 });
            result.Add(new OrderStatusCode() { Bvin = "F37EC405-1EC6-4a91-9AC4-6836215FBBBC", SystemCode = true, StatusName = "Received", SortOrder = 2 });
            result.Add(new OrderStatusCode() { Bvin = "e42f8c28-9078-47d6-89f8-032c9a6e1cce", SystemCode = true, StatusName = "Ready for Payment", SortOrder = 3 });
            result.Add(new OrderStatusCode() { Bvin = "0c6d4b57-3e46-4c20-9361-6b0e5827db5a", SystemCode = true, StatusName = "Ready for Shipping", SortOrder = 5 });
            result.Add(new OrderStatusCode() { Bvin=  "09D7305D-BD95-48d2-A025-16ADC827582A", SystemCode = true, StatusName = "Complete", SortOrder = 6});

            return result;
        }

        public static OrderStatusCode FindByBvin(string bvin)
        {
            foreach (OrderStatusCode o in FindAll())
            {
                if (o.Bvin == bvin)
                {
                    return o;
                }
            }
            return null;            
        }

	}
}
