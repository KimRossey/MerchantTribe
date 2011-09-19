using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.CommerceDTO.v1.Orders;
using MerchantTribe.CommerceDTO.v1.Contacts;
using MerchantTribe.CommerceDTO.v1;

namespace MerchantTribe.Commerce.Orders
{
    public class OrderSnapshot
    {

        // Basics
        public int Id { get; set; } // not used as primary key, only for insert order in SQL pages
        public string bvin { get; set; } // Primary Key
        public long StoreId { get; set; }
        public DateTime LastUpdatedUtc { get; set; }
        public DateTime TimeOfOrderUtc { get; set; }
        public string OrderNumber { get; set; }
        public string ThirdPartyOrderId { get; set; }

        public string AcumaticaId
        {
            get
            {
                return this.CustomProperties.GetProperty("bvsoftware", "acumaticaid").Trim();
            }
            set
            {
                this.CustomProperties.SetProperty("bvsoftware", "acumaticaid", value);
            }
        }

        public string UserEmail { get; set; }
        public string UserID { get; set; }
        public CustomPropertyCollection CustomProperties { get; set; }

        // Status        
        public OrderPaymentStatus PaymentStatus { get; set; }
        public OrderShippingStatus ShippingStatus { get; set; }
        public bool IsPlaced { get; set; }
        public string StatusCode { get; set; }
        public string StatusName { get; set; }

        // Addresses
        public Contacts.Address BillingAddress { get; set; }
        public Contacts.Address ShippingAddress { get; set; }

        // Totals                        
        public decimal TotalTax { get; set; }
        public decimal TotalTax2 { get; set; }
        public decimal TotalOrderBeforeDiscounts { get; set; }
        public decimal TotalShippingBeforeDiscounts { get; set; }
        public decimal TotalShippingDiscounts { get; set; }
        public decimal TotalOrderDiscounts { get; set; }
        public decimal TotalHandling { get; set; }
        public decimal TotalGrand { get; set; }

        // Others
        public string AffiliateID { get; set; }
        public decimal FraudScore { get; set; }
        public string Instructions { get; set; }
        public string ShippingMethodId { get; set; }
        public string ShippingMethodDisplayName { get; set; }
        public string ShippingProviderId { get; set; }
        public string ShippingProviderServiceCode { get; set; }

        public OrderSnapshot()
        {
            this.bvin = string.Empty;
            this.StoreId = 0;
            this.LastUpdatedUtc = DateTime.UtcNow;
            this.TimeOfOrderUtc = DateTime.MinValue;
            this.OrderNumber = string.Empty;
            this.ThirdPartyOrderId = string.Empty;
            this.UserEmail = string.Empty;
            this.UserID = string.Empty;
            this.CustomProperties = new CustomPropertyCollection();

            this.PaymentStatus = OrderPaymentStatus.Unknown;
            this.ShippingStatus = OrderShippingStatus.Unknown;
            this.IsPlaced = false;
            this.StatusCode = string.Empty;
            this.StatusName = string.Empty;

            this.BillingAddress = new Contacts.Address();
            this.ShippingAddress = new Contacts.Address();

            this.TotalTax = 0m;
            this.TotalTax2 = 0m;
            this.TotalOrderBeforeDiscounts = 0m;
            this.TotalShippingBeforeDiscounts = 0m;
            this.TotalShippingDiscounts = 0m;
            this.TotalOrderDiscounts = 0m;
            this.TotalHandling = 0m;
            this.TotalGrand = 0m;

            this.AffiliateID = string.Empty;
            this.FraudScore = -1m;
            this.Instructions = string.Empty;
            this.ShippingMethodId = string.Empty;
            this.ShippingMethodDisplayName = string.Empty;
            this.ShippingProviderId = string.Empty;
            this.ShippingProviderServiceCode = string.Empty;        
        
        }

        // DTO
        public OrderSnapshotDTO ToDto()
        {
            OrderSnapshotDTO dto = new OrderSnapshotDTO();

            dto.AffiliateID = this.AffiliateID ?? string.Empty;
            dto.BillingAddress = this.BillingAddress.ToDto();
            dto.bvin = this.bvin ?? string.Empty;
            dto.CustomProperties = new List<CustomPropertyDTO>();
            foreach (CustomProperty prop in this.CustomProperties)
            {
                dto.CustomProperties.Add(prop.ToDto());
            }
            dto.FraudScore = this.FraudScore;
            dto.Id = this.Id;
            dto.Instructions = this.Instructions ?? string.Empty;
            dto.IsPlaced = this.IsPlaced;
            dto.LastUpdatedUtc = this.LastUpdatedUtc;
            dto.OrderNumber = this.OrderNumber ?? string.Empty;
            dto.PaymentStatus = (OrderPaymentStatusDTO)((int)this.PaymentStatus);
            dto.ShippingAddress = this.ShippingAddress.ToDto();
            dto.ShippingMethodDisplayName = this.ShippingMethodDisplayName ?? string.Empty;
            dto.ShippingMethodId = this.ShippingMethodId ?? string.Empty;
            dto.ShippingProviderId = this.ShippingProviderId ?? string.Empty;
            dto.ShippingProviderServiceCode = this.ShippingProviderServiceCode ?? string.Empty;
            dto.ShippingStatus = (OrderShippingStatusDTO)((int)this.ShippingStatus);
            dto.StatusCode = this.StatusCode ?? string.Empty;
            dto.StatusName = this.StatusName ?? string.Empty;
            dto.StoreId = this.StoreId;
            dto.ThirdPartyOrderId = this.ThirdPartyOrderId ?? string.Empty;
            dto.TimeOfOrderUtc = this.TimeOfOrderUtc;
            dto.TotalGrand = this.TotalGrand;
            dto.TotalHandling = this.TotalHandling;
            dto.TotalOrderBeforeDiscounts = this.TotalOrderBeforeDiscounts;
            dto.TotalOrderDiscounts = this.TotalOrderDiscounts;
            dto.TotalShippingBeforeDiscounts = this.TotalShippingBeforeDiscounts;
            dto.TotalShippingDiscounts = this.TotalShippingDiscounts;
            dto.TotalTax = this.TotalTax;
            dto.TotalTax2 = this.TotalTax2;
            dto.UserEmail = this.UserEmail ?? string.Empty;
            dto.UserID = this.UserID ?? string.Empty;

            return dto;
        }        
    }
}
