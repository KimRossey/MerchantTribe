using System;
using System.Data;
using System.Collections.ObjectModel;
using MerchantTribe.CommerceDTO.v1.Contacts;

namespace MerchantTribe.Commerce.Contacts
{
	public class AffiliateReferral
	{
        public long Id { get; set; }
        public long StoreId { get; set; }
        public DateTime TimeOfReferralUtc { get; set; }
		public long AffiliateId {get;set;}
		public string ReferrerUrl {get;set;}

        public AffiliateReferral()
        {
            this.Id = 0;
            this.StoreId = 0;
            this.TimeOfReferralUtc = DateTime.UtcNow;
            this.AffiliateId = 0;
            this.ReferrerUrl = string.Empty;
        }
	
        //DTO
        public AffiliateReferralDTO ToDto()
        {
            AffiliateReferralDTO dto = new AffiliateReferralDTO();

            dto.Id = this.Id;
            dto.StoreId = this.StoreId;
            dto.TimeOfReferralUtc = this.TimeOfReferralUtc;
            dto.AffiliateId = this.AffiliateId;
            dto.ReferrerUrl = this.ReferrerUrl;

            return dto;
        }
        public void FromDto(AffiliateReferralDTO dto)
        {
            if (dto == null) return;

            this.Id = dto.Id;
            this.StoreId = dto.StoreId;
            this.TimeOfReferralUtc = dto.TimeOfReferralUtc;
            this.AffiliateId = dto.AffiliateId;
            this.ReferrerUrl = dto.ReferrerUrl;
        }
	}
}
