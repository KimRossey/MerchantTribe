using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using MerchantTribe.CommerceDTO.v1.Catalog;

namespace MerchantTribe.Commerce.Catalog
{
    [Serializable]
	public class ProductPropertyChoice
	{
        public long Id {get;set;}
        public long StoreId { get; set; }
        public long PropertyId {get;set;}
		public string ChoiceName {get;set;}
		public int SortOrder {get;set;}
        public DateTime LastUpdated {get;set;}

        public ProductPropertyChoice()
        {
            this.Id = 0;
            this.StoreId = 0;
            this.ChoiceName = string.Empty;
            this.SortOrder = 0;
            this.LastUpdated = DateTime.UtcNow;
        }
					
        // DTO
        public ProductPropertyChoiceDTO ToDto()
        {
            ProductPropertyChoiceDTO dto = new ProductPropertyChoiceDTO();

            dto.ChoiceName = this.ChoiceName;
            dto.Id = this.Id;
            dto.LastUpdated = this.LastUpdated;
            dto.PropertyId = this.PropertyId;
            dto.SortOrder = this.SortOrder;
            dto.StoreId = this.StoreId;
            
            return dto;
        }
        public void FromDto(ProductPropertyChoiceDTO dto)
        {
            if (dto == null) return;

            this.ChoiceName = dto.ChoiceName ?? string.Empty;
            this.Id = dto.Id;
            this.LastUpdated = dto.LastUpdated;
            this.PropertyId = dto.PropertyId;
            this.SortOrder = dto.SortOrder;
            this.StoreId = dto.StoreId;
        }

	}
}

