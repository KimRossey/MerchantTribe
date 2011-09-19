using System;
using System.Data;
using System.Collections.ObjectModel;
using MerchantTribe.CommerceDTO.v1.Catalog;

namespace MerchantTribe.Commerce.Catalog
{

	public class CategoryProductAssociation
	{
        public long Id { get; set; }
		public string CategoryId {get;set;}
		public string ProductId {get;set;}
		public int SortOrder {get;set;}
        public long StoreId {get;set;}

        public CategoryProductAssociation()
		{
            this.Id = 0;
            this.CategoryId = string.Empty;
            this.ProductId = string.Empty;
            this.SortOrder = 0;
            this.StoreId = 0;
		}

        //DTO
        public CategoryProductAssociationDTO ToDto()
        {
            CategoryProductAssociationDTO dto = new CategoryProductAssociationDTO();

            dto.Id = this.Id;
            dto.CategoryId = this.CategoryId;
            dto.ProductId = this.ProductId;
            dto.SortOrder = this.SortOrder;
            dto.StoreId = this.StoreId;
            
            return dto;
        }
        public void FromDto(CategoryProductAssociationDTO dto)
        {
            if (dto == null) return;

            this.Id = dto.Id;
            this.CategoryId = dto.CategoryId;
            this.ProductId = dto.ProductId;
            this.SortOrder = dto.SortOrder;
            this.StoreId = dto.StoreId;
        }
	}
}


