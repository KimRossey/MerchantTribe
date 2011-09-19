using System;
using System.Data;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using MerchantTribe.CommerceDTO.v1.Catalog;

namespace MerchantTribe.Commerce.Catalog
{
    [Serializable]
	public class ProductVolumeDiscount
	{
        public string Bvin { get; set; }
        public long StoreId { get; set; }
        public DateTime LastUpdated { get; set; }
		public string ProductId {get;set;}
		public int Qty {get;set;}
		public decimal Amount {get;set;}
		public ProductVolumeDiscountType DiscountType {get;set;}

        public ProductVolumeDiscount()
        {
            this.Bvin = string.Empty;
            this.StoreId = 0;
            this.LastUpdated = DateTime.UtcNow;
            this.ProductId = string.Empty;
            this.Qty = -1;
            this.Amount = 0m;
            this.DiscountType = ProductVolumeDiscountType.None;
        }
	
        //DTO
        public ProductVolumeDiscountDTO ToDto()
        {
            ProductVolumeDiscountDTO dto = new ProductVolumeDiscountDTO();

            dto.Amount = this.Amount;
            dto.StoreId = this.StoreId;
            dto.Bvin = this.Bvin;
            dto.DiscountType = (ProductVolumeDiscountTypeDTO)((int)this.DiscountType);
            dto.LastUpdated = this.LastUpdated;
            dto.ProductId = this.ProductId;
            dto.Qty = this.Qty;
            
            return dto;
        }
        public void FromDto(ProductVolumeDiscountDTO dto)
        {
            if (dto == null) return;

            this.Amount = dto.Amount;
            this.StoreId = dto.StoreId;
            this.Bvin = dto.Bvin ?? string.Empty;
            this.DiscountType = (ProductVolumeDiscountType)((int)dto.DiscountType);
            this.LastUpdated = dto.LastUpdated;
            this.ProductId = dto.ProductId ?? string.Empty;
            this.Qty = this.Qty;
        }	

	}

}

