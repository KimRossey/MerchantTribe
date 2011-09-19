using System;
using System.Collections.ObjectModel;
using System.Data;

namespace MerchantTribe.Commerce.Orders
{
	public class OrderSearchCriteria
	{								
		public bool IsPlaced {get;set;}
		public string Keyword {get;set;}
		public string StatusCode {get;set;}
		public OrderPaymentStatus PaymentStatus {get;set;}
		public OrderShippingStatus ShippingStatus {get;set;}
		public DateTime EndDateUtc {get;set;}
		public DateTime StartDateUtc {get;set;}
		public string AffiliateId {get;set;}
		public string OrderNumber {get;set;}
        public bool SortDescending { get; set; }

		public OrderSearchCriteria()
		{
            this.IsPlaced = true;
            this.Keyword = string.Empty;
            this.StatusCode = string.Empty;
            this.PaymentStatus = OrderPaymentStatus.Unknown;
            this.ShippingStatus = OrderShippingStatus.Unknown;
            this.StartDateUtc = new DateTime(1900, 1, 1);
		    this.EndDateUtc = new DateTime(3000, 1, 1);
            this.AffiliateId = string.Empty;
            this.OrderNumber = string.Empty;
            this.SortDescending = false;
		}

	}
}

