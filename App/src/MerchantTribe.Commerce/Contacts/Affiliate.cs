using System;
using System.Data;
using System.Collections.Generic;
using System.Xml;
using System.Xml.XPath;
using System.IO;
using System.Xml.Serialization;
using System.Web;
using System.ComponentModel;
using MerchantTribe.CommerceDTO.v1.Contacts;

namespace MerchantTribe.Commerce.Contacts
{

	public class Affiliate
	{	
        public long Id {get;set;}
        public long StoreId { get; set; }
        public System.DateTime LastUpdatedUtc {get;set;}
		public bool Enabled {get;set;}
		public string ReferralId {get;set;}
		public string DisplayName {get;set;}
		public Contacts.Address Address {get;set;}
		public decimal CommissionAmount {get;set;}
		public Contacts.AffiliateCommissionType CommissionType {get;set;}
		public int ReferralDays {get;set;}
		public string TaxId {get;set;}
		public string DriversLicenseNumber {get;set;}
		public string WebSiteUrl {get;set;}
		public string CustomThemeName {get;set;}
		public string Notes {get;set;}
        public List<AffiliateContact> Contacts { get; set; }

		public Affiliate()
		{
            this.Id = 0;
            this.StoreId = 0;
            this.Enabled = false;
		    this.ReferralId = string.Empty;
		    this.DisplayName = string.Empty;
		    this.Address = new Contacts.Address();
		    this.CommissionAmount = 0;
		    this.CommissionType = AffiliateCommissionType.PercentageCommission;
		    this.ReferralDays = 30;
		    this.TaxId = string.Empty;
		    this.DriversLicenseNumber = string.Empty;
		    this.WebSiteUrl = string.Empty;
		    this.CustomThemeName = string.Empty;
		    this.Notes = string.Empty;
            this.LastUpdatedUtc = System.DateTime.UtcNow;
            this.Contacts = new List<AffiliateContact>();
		}

        public string GetDefaultLink(Accounts.Store currentStore)
        {
            string result = "";
            result = currentStore.RootUrl();
            result += "?" + WebAppSettings.AffiliateQueryStringName + "=" + this.ReferralId;
            return result;
        }
       
        //public static Collection<Affiliate> FindByUserId(string bvin)
        //{
        //    return Mapper.FindByUserId(bvin);
        //}
        //public static Collection<Affiliate> FindEnabledByUserId(string bvin)
        //{
        //    Collection<Affiliate> enabledAffiliates = new Collection<Affiliate>();
        //    foreach (Contacts.Affiliate item in Mapper.FindByUserId(bvin)) {
        //        if (item.Enabled) {
        //            enabledAffiliates.Add(item);
        //        }
        //    }
        //    return enabledAffiliates;
        //}
        
		//DTO
        public AffiliateDTO ToDto()
        {
            AffiliateDTO dto = new AffiliateDTO();

            dto.Id = this.Id;
            dto.StoreId = this.StoreId;
            dto.Enabled = this.Enabled;
            dto.ReferralId = this.ReferralId;
            dto.DisplayName = this.DisplayName;
            dto.Address = this.Address.ToDto();
            dto.CommissionAmount = this.CommissionAmount;
            dto.CommissionType = (AffiliateCommissionTypeDTO)((int)this.CommissionType);
            dto.ReferralDays = this.ReferralDays;
            dto.TaxId = this.TaxId;
            dto.DriversLicenseNumber = this.DriversLicenseNumber;
            dto.WebSiteUrl = this.WebSiteUrl;
            dto.CustomThemeName = this.CustomThemeName;
            dto.Notes = this.Notes;
            dto.LastUpdatedUtc = this.LastUpdatedUtc;
            foreach (AffiliateContact contact in this.Contacts)
            {
                dto.Contacts.Add(contact.ToDto());
            }            

            return dto;
        }
        public void FromDto(AffiliateDTO dto)
        {
            if (dto == null) return;

            this.Id = dto.Id;
            this.StoreId = dto.StoreId;
            this.Enabled = dto.Enabled;
            this.ReferralId = dto.ReferralId;
            this.DisplayName = dto.DisplayName;
            this.Address.FromDto(dto.Address);
            this.CommissionAmount = dto.CommissionAmount;
            this.CommissionType = (AffiliateCommissionType)((int)dto.CommissionType);
            this.ReferralDays = dto.ReferralDays;
            this.TaxId = dto.TaxId;
            this.DriversLicenseNumber = dto.DriversLicenseNumber;
            this.WebSiteUrl = dto.WebSiteUrl;
            this.CustomThemeName = dto.CustomThemeName;
            this.Notes = dto.Notes;
            this.LastUpdatedUtc = dto.LastUpdatedUtc;
            
            this.Contacts.Clear();
            foreach (AffiliateContactDTO contact in dto.Contacts)
            {
                AffiliateContact c = new AffiliateContact();
                c.FromDto(contact);
                this.Contacts.Add(c);
            }            
        }
	}
}
