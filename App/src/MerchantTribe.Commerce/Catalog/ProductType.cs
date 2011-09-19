using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Collections.Generic;
using MerchantTribe.CommerceDTO.v1.Catalog;

namespace MerchantTribe.Commerce.Catalog
{
    [Serializable]
	public class ProductType
	{
        public string Bvin { get; set; }
        public long StoreId { get; set; }
        public DateTime LastUpdated { get; set; }
		public bool IsPermanent {get;set;}
		public string ProductTypeName {get;set;}

        public ProductType()
        {
            this.Bvin = string.Empty;
            this.StoreId = 0;
            this.LastUpdated = DateTime.UtcNow;
            this.ProductTypeName = string.Empty;
            this.IsPermanent = false;
        }
	
        // DTO
        public ProductTypeDTO ToDto()
        {
            ProductTypeDTO dto = new ProductTypeDTO();

            dto.Bvin = this.Bvin;
            dto.StoreId = this.StoreId;
            dto.IsPermanent = this.IsPermanent;
            dto.LastUpdated = this.LastUpdated;
            dto.ProductTypeName = this.ProductTypeName;

            return dto;
        }
        public void FromDto(ProductTypeDTO dto)
        {
            if (dto == null) return;

            this.Bvin = dto.Bvin ?? string.Empty;
            this.StoreId = dto.StoreId;
            this.IsPermanent = dto.IsPermanent;
            this.LastUpdated = dto.LastUpdated;
            this.ProductTypeName = dto.ProductTypeName ?? string.Empty;
        }
				
	}

}
