using System;
using System.Data;
using System.Collections.ObjectModel;
using MerchantTribe.CommerceDTO.v1.Contacts;

namespace MerchantTribe.Commerce.Contacts
{

	public class PriceGroup
	{
        public long StoreId { get; set; }
        public string Bvin { get; set; }
        public DateTime LastUpdated { get; set; }		
		public string Name {get;set;}
		public PricingTypes PricingType {get;set;}
		public decimal AdjustmentAmount {get;set;}

        public PriceGroup()
        {
            this.StoreId = 0;
            this.Bvin = string.Empty;
            this.LastUpdated = DateTime.UtcNow;
            this.Name = string.Empty;
            this.PricingType = PricingTypes.PercentageOffListPrice;
            this.AdjustmentAmount = 0m;
        }

        public decimal GetAdjustedPriceForThisGroup(decimal price, decimal msrp, decimal cost)
        {
            decimal result = price;

            switch (this.PricingType)
            {
                case Contacts.PricingTypes.AmountAboveCost:
                    result = Utilities.Money.ApplyIncreasedAmount(cost, this.AdjustmentAmount);
                    break;
                case Contacts.PricingTypes.AmountOffListPrice:
                    result = Utilities.Money.ApplyDiscountAmount(msrp, this.AdjustmentAmount);
                    break;
                case Contacts.PricingTypes.AmountOffSitePrice:
                    result = Utilities.Money.ApplyDiscountAmount(price, this.AdjustmentAmount);
                    break;
                case Contacts.PricingTypes.PercentageAboveCost:
                    result = Utilities.Money.ApplyIncreasedPercent(cost, this.AdjustmentAmount);
                    break;
                case Contacts.PricingTypes.PercentageOffListPrice:
                    result = Utilities.Money.ApplyDiscountPercent(msrp, this.AdjustmentAmount);
                    break;
                case Contacts.PricingTypes.PercentageOffSitePrice:
                    result = Utilities.Money.ApplyDiscountPercent(price, this.AdjustmentAmount);
                    break;
            }

            return result;
        }

        public PriceGroupDTO ToDto()
        {
            PriceGroupDTO dto = new PriceGroupDTO();

            dto.AdjustmentAmount = this.AdjustmentAmount;
            dto.Bvin = this.Bvin;
            dto.LastUpdated = this.LastUpdated;
            dto.Name = this.Name;
            dto.PricingType = (PricingTypesDTO)((int)this.PricingType);
            dto.StoreId = this.StoreId;            

            return dto;
        }
        public void FromDto(PriceGroupDTO dto)
        {
            this.AdjustmentAmount = dto.AdjustmentAmount;
            this.Bvin = dto.Bvin;
            this.LastUpdated = dto.LastUpdated;
            this.Name = dto.Name;
            this.PricingType = (PricingTypes)((int)dto.PricingType);
            this.StoreId = dto.StoreId;         
        }
	}
}
