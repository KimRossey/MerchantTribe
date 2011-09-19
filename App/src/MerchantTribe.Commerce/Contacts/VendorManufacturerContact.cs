using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.CommerceDTO.v1.Contacts;

namespace MerchantTribe.Commerce.Contacts
{
    public class VendorManufacturerContact
    {
        public long Id { get; set; }
        public long StoreId { get; set; }
        public string VendorManufacturerId { get; set; }
        public string UserId { get; set; }
        
        public VendorManufacturerContact()
        {
            Id = 0;
            VendorManufacturerId = string.Empty;
            UserId = string.Empty;
            StoreId = 0;
        }

        //DTO
        public VendorManufacturerContactDTO ToDto()
        {
            VendorManufacturerContactDTO dto = new VendorManufacturerContactDTO();

            dto.Id = this.Id;
            dto.StoreId = this.StoreId;
            dto.UserId = this.UserId;
            dto.VendorManufacturerId = this.VendorManufacturerId;

            return dto;
        }
        public void FromDto(VendorManufacturerContactDTO dto)
        {
            if (dto == null) return;

            this.Id = dto.Id;
            this.StoreId = dto.StoreId;
            this.UserId = dto.UserId;
            this.VendorManufacturerId = dto.VendorManufacturerId;
        }
    }
}
