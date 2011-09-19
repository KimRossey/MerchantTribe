using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Web.Geography;
using MerchantTribe.CommerceDTO.v1.Taxes;

namespace MerchantTribe.Commerce.Taxes
{
    public class Tax : ITaxRate
    {
        public long Id { get; set; }
        public long StoreId { get; set; }
        public string CountryName { get; set; }
        public string RegionAbbreviation { get; set; }
        public string PostalCode { get; set; }
        public long TaxScheduleId { get; set; }
        public decimal Rate { get; set; }
        public bool ApplyToShipping { get; set; }

        public Tax()
        {
            Id = 0;
            StoreId = 0;
            CountryName = string.Empty;
            RegionAbbreviation = string.Empty;
            PostalCode = string.Empty;
            TaxScheduleId = 0;
            Rate = 0m;
            ApplyToShipping = false;
        }

        protected bool AppliesToItem(ITaxable item, IAddress address)
        {
            bool result = false;

            if (item.TaxScheduleId() == this.TaxScheduleId)
            {
                if (address.CountryData.Name.ToLowerInvariant() == this.CountryName.ToLowerInvariant())
                {
                    if (this.RegionAbbreviation == "" ||
                        address.RegionData.Abbreviation.ToLowerInvariant() == this.RegionAbbreviation.ToLowerInvariant())
                    {
                        if (this.PostalCode == "" ||
                            address.PostalCode.ToLowerInvariant() == this.PostalCode.ToLowerInvariant())
                        {
                            result = true;
                        }
                    }
                }
            }

            return result;
        }

        #region ITaxRate Members

        void ITaxRate.TaxItem(ITaxable item, IAddress address)
        {
            if (AppliesToItem(item, address))
            {
                if (this.ApplyToShipping)
                {
                    item.IncrementTaxValue((item.TaxableValue() + item.TaxableShippingValue()) * (this.Rate / 100M));
                }
                else
                {
                    item.IncrementTaxValue(item.TaxableValue() * (this.Rate / 100M));
                }
            }
        }

        #endregion 

        //DTO
        public TaxDTO ToDto()
        {
            TaxDTO dto = new TaxDTO();

            dto.ApplyToShipping = this.ApplyToShipping;
            dto.CountryName = this.CountryName;
            dto.Id = this.Id;
            dto.PostalCode = this.PostalCode;
            dto.Rate = this.Rate;
            dto.RegionAbbreviation = this.RegionAbbreviation;
            dto.StoreId = this.StoreId;
            dto.TaxScheduleId = this.TaxScheduleId;
            
            return dto;
        }
        public void FromDto(TaxDTO dto)
        {
            if (dto == null) return;

            this.ApplyToShipping = dto.ApplyToShipping;
            this.CountryName = dto.CountryName;
            this.Id = dto.Id;
            this.PostalCode = dto.PostalCode;
            this.Rate = dto.Rate;
            this.RegionAbbreviation = dto.RegionAbbreviation;
            this.StoreId = dto.StoreId;
            this.TaxScheduleId = dto.TaxScheduleId;
        }
    }
}
