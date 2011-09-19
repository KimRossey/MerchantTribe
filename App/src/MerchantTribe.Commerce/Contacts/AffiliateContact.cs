using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.CommerceDTO.v1.Contacts;

namespace MerchantTribe.Commerce.Contacts
{
    public class AffiliateContact
    {
        public long Id { get; set; }
        public long StoreId { get; set; }
        public string AffiliateId { get; set; }
        public string UserId { get; set; }

        public AffiliateContact()
        {
            Id = 0;
            AffiliateId = string.Empty;
            UserId = string.Empty;
            StoreId = 0;
        }

        //DTO
        public AffiliateContactDTO ToDto()
        {
            AffiliateContactDTO dto = new AffiliateContactDTO();
            
            dto.Id = this.Id;
            dto.AffiliateId = this.AffiliateId;
            dto.UserId = this.UserId;
            dto.StoreId = this.StoreId;

            return dto;
        }
        public void FromDto(AffiliateContactDTO dto)
        {
            if (dto == null) return;

            this.Id = dto.Id;
            this.AffiliateId = dto.AffiliateId;
            this.UserId = dto.UserId;
            this.StoreId = dto.StoreId;

        }
    }
}
