using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.CommerceDTO.v1.Catalog;

namespace MerchantTribe.Commerce.Catalog
{
    public class ProductRelationship
    {
        public long Id { get; set; }
        public long StoreId { get; set; }
        public string ProductId { get; set; }
        public string RelatedProductId { get; set; }
        public bool IsSubstitute { get; set; }
        public int SortOrder { get; set; }
        public string MarketingDescription { get; set; }
        

        public ProductRelationship()
        {
            this.Id = 0;
            this.StoreId = 0;
            this.ProductId = string.Empty;
            this.RelatedProductId = string.Empty;
            this.IsSubstitute = false;
            this.SortOrder = 0;
            this.MarketingDescription = string.Empty;
        }

        //DTO
        public ProductRelationshipDTO ToDto()
        {
            ProductRelationshipDTO dto = new ProductRelationshipDTO();

            dto.Id = this.Id;
            dto.StoreId = this.StoreId;
            dto.ProductId = this.ProductId;
            dto.RelatedProductId = this.RelatedProductId;
            dto.IsSubstitute = this.IsSubstitute;
            dto.SortOrder = this.SortOrder;
            dto.MarketingDescription = this.MarketingDescription;

            return dto;
        }
        public void FromDto(ProductRelationshipDTO dto)
        {
            if (dto == null) return;

            this.Id = dto.Id;
            this.StoreId = dto.StoreId;
            this.ProductId = dto.ProductId;
            this.RelatedProductId = dto.RelatedProductId;
            this.IsSubstitute = dto.IsSubstitute;
            this.SortOrder = dto.SortOrder;
            this.MarketingDescription = dto.MarketingDescription;
        }
    }
}
