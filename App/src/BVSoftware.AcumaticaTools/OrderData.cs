using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BVSoftware.AcumaticaTools.Acumatica;

namespace BVSoftware.AcumaticaTools
{
    public class OrderData
    {
        public string BVOrderNumber { get; set; }
        public string AcumaticaOrderNumber { get; set; }
        public string StatusCode { get; set; }
        public string Notes { get; set; }

        public DateTime? TimeOfOrder { get; set; }
        public CustomerData Customer { get; set; }
        public MerchantTribe.Web.Geography.IAddress ShippingAddress { get; set; }

        public List<OrderItemData> Items {get;set;}

        public OrderShippingData Shipping { get; set; }
        public PaymentInformation Payment { get; set; }
        public string PaymentMethodName { get; set; }
        public string PaymentMethodId { get; set; }

        public decimal ShippingTotal { get; set; }
        public decimal TaxTotal { get; set; }
        public decimal GrandTotal { get; set; }

        public List<OrderShipmentData> Shipments { get; set; }

        public OrderData()
        {
            AcumaticaOrderNumber = string.Empty;
            Items = new List<OrderItemData>();
            Shipping = new OrderShippingData();
            Customer = new CustomerData();
            Payment = new PaymentInformation();
            Notes = string.Empty;
            StatusCode = string.Empty;
            PaymentMethodName = string.Empty;
            PaymentMethodId = string.Empty;
            Shipments = new List<OrderShipmentData>();
        }

        internal void LoadFromErp(SO301000Content order)
        {
			this.BVOrderNumber = order.OrderSummary.CustomerOrder.Value ?? string.Empty;
			this.AcumaticaOrderNumber = order.OrderSummary.OrderNbr.Value ?? string.Empty;

			if (!String.IsNullOrEmpty(order.OrderSummary.Date.Value))
				this.TimeOfOrder = DateTime.Parse(order.OrderSummary.Date.Value);
			
			this.GrandTotal = Decimal.Parse(order.OrderSummary.OrderTotal.Value ?? "0");			
		    this.TaxTotal = Decimal.Parse(order.OrderSummary.TaxTotal.Value ?? "0");					    
            this.Shipping.ShippingTotal = Decimal.Parse(order.Totals.Freight.Value ?? "0");
            this.Customer.AcumaticaId = order.OrderSummary.Customer.Value ?? string.Empty;

            this.Notes = order.OrderSummary.NoteText.Value ?? string.Empty;
			this.StatusCode = order.OrderSummary.Status.Value ?? string.Empty;
									
			this.ShippingTotal = Decimal.Parse(order.Totals.PremiumFreight.Value ?? "0");					                                                
            //schema.Totals.Freight,	

            // Billing Address
            if (order.FinancialSettingsOverrideAddress != null)
            {
                if (!String.IsNullOrEmpty(order.FinancialSettingsOverrideAddress.AddressLine1.Value))
                {
                    this.Customer.BillingAddress = new MerchantTribe.Web.Geography.SimpleAddress();
                    this.Customer.BillingAddress.Street = order.FinancialSettingsOverrideAddress.AddressLine1.Value ?? string.Empty;
                    this.Customer.BillingAddress.Street += order.FinancialSettingsOverrideAddress.AddressLine2.Value ?? string.Empty;
                    this.Customer.BillingAddress.City = order.FinancialSettingsOverrideAddress.City.Value ?? string.Empty;
                    this.Customer.BillingAddress.CountryData.Name = order.FinancialSettingsOverrideAddress.Country.Value ?? string.Empty;
                    this.Customer.BillingAddress.PostalCode = order.FinancialSettingsOverrideAddress.PostalCode.Value ?? string.Empty;
                    this.Customer.BillingAddress.RegionData.Name = order.FinancialSettingsOverrideAddress.State.Value ?? string.Empty;
                }
            }
              
                
            // Shipping Address
            if (order.ShippingSettingsOverrideAddress != null)
            {
                if (!String.IsNullOrEmpty(order.ShippingSettingsOverrideAddress.AddressLine1.Value))
                {
                    this.ShippingAddress = new MerchantTribe.Web.Geography.SimpleAddress();
                    this.ShippingAddress.Street = order.ShippingSettingsOverrideAddress.AddressLine1.Value ?? string.Empty;
                    this.ShippingAddress.Street += order.ShippingSettingsOverrideAddress.AddressLine2.Value ?? string.Empty;
                    this.ShippingAddress.City = order.ShippingSettingsOverrideAddress.City.Value ?? string.Empty;
                    this.ShippingAddress.CountryData.Name = order.ShippingSettingsOverrideAddress.Country.Value ?? string.Empty;
                    this.ShippingAddress.PostalCode = order.ShippingSettingsOverrideAddress.PostalCode.Value ?? string.Empty;
                    this.ShippingAddress.RegionData.Name = order.ShippingSettingsOverrideAddress.State.Value ?? string.Empty;
                }
            }

            this.PaymentMethodId = order.PaymentSettings.PaymentMethod.Value ?? string.Empty;
            this.PaymentMethodName = order.PaymentSettings.PaymentMethodIdentifier.Value ?? string.Empty;
            

                // Line Items
                //schema.DocumentDetails.InventoryID,
                //schema.DocumentDetails.Quantity,
                //schema.DocumentDetails.QtyOnShipments,    
                //schema.DocumentDetails.UnitPrice,
                //schema.DocumentDetails.Discount,
                //schema.DocumentDetails.DiscountAmount,
                //schema.DocumentDetails.DiscUnitPrice,
                //schema.DocumentDetails.ExtPrice,
                //schema.DocumentDetails.LineDescription,
                //schema.DocumentDetails.ShipComplete,
                //schema.DocumentDetails.ShipOn,
                //schema.DocumentDetails.Canceled,
                                                          

                //schema.Shipments.ShipmentNbr,
                //schema.Shipments.ShipmentType,
                //schema.Shipments.Status,
                //schema.Shipments.ShippedQty,
                //schema.CarrierRatesPackages.BoxID,
                //schema.CarrierRatesPackages.LineNbr,

            //if (!String.IsNullOrEmpty(order.Payments.PaymentMethod.Value))
            //{
            //    if (order.Payments.PaymentMethod.Value == "CreditCard")
            //    {
                    
            //    }
            //}
        }
    }
}
