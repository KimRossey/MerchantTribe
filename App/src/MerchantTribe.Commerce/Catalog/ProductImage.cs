using System;
using System.Data;
using System.Collections.ObjectModel;
using MerchantTribe.CommerceDTO.v1.Catalog;

namespace MerchantTribe.Commerce.Catalog
{
    [Serializable]
	public class ProductImage
	{
        public string Bvin { get; set; }
        public DateTime LastUpdatedUtc { get; set; }
		public string ProductId {get;set;}
		public string FileName {get;set;}
		public string Caption {get;set;}
		public string AlternateText {get;set;}
		public int SortOrder {get;set;}
        public long StoreId { get; set; }

        public ProductImage()
        {
            this.Bvin = string.Empty;
            this.LastUpdatedUtc = DateTime.UtcNow;
            this.ProductId = string.Empty;
            this.FileName = string.Empty;
            this.Caption = string.Empty;
            this.AlternateText = string.Empty;
            this.SortOrder = -1;
            this.StoreId = 0;
        }

        public ProductImage Clone()
        {
            ProductImage result = new ProductImage();

            result.AlternateText = this.AlternateText;
            result.Bvin = string.Empty;
            result.Caption = this.Caption;
            result.FileName = this.FileName;
            result.LastUpdatedUtc = this.LastUpdatedUtc;
            result.ProductId = this.ProductId;
            result.SortOrder = this.SortOrder;
            result.StoreId = this.StoreId;

            return result;
        }

        // DTO
        public ProductImageDTO ToDto()
        {
            ProductImageDTO dto = new ProductImageDTO();

            dto.AlternateText = this.AlternateText;
            dto.Bvin = this.Bvin;
            dto.Caption = this.Caption;
            dto.FileName = this.FileName;
            dto.LastUpdatedUtc = this.LastUpdatedUtc;
            dto.ProductId = this.ProductId;
            dto.SortOrder = this.SortOrder;
            dto.StoreId = this.StoreId;
            
            return dto;
        }
        public void FromDto(ProductImageDTO dto)
        {
            if (dto == null) return;

            this.AlternateText = dto.AlternateText ?? string.Empty;
            this.Bvin = dto.Bvin ?? string.Empty;
            this.Caption = dto.Caption ?? string.Empty;
            this.FileName = dto.FileName ?? string.Empty;
            this.LastUpdatedUtc = dto.LastUpdatedUtc;
            this.ProductId = dto.ProductId ?? string.Empty;
            this.SortOrder = dto.SortOrder;
            this.StoreId = dto.StoreId;
        }



	}

}
