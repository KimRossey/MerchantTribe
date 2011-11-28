using System;
using System.Data;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Linq;
using MerchantTribe.Commerce.Marketing;
using System.Text;
using MerchantTribe.CommerceDTO.v1.Marketing;
using MerchantTribe.CommerceDTO.v1.Catalog;
using MerchantTribe.CommerceDTO.v1.Shipping;
using MerchantTribe.CommerceDTO.v1.Orders;
using MerchantTribe.CommerceDTO.v1;

namespace MerchantTribe.Commerce.Catalog
{
    public class WishListItem
    {
        public long Id { get; set; }
        public long StoreId { get; set; }
        public string CustomerId { get; set; }
        public System.DateTime LastUpdatedUtc { get; set; }                        
        public string ProductId { get; set; }                
        public int Quantity { get; set; }
        public Catalog.OptionSelectionList SelectionData { get; set; }

        private void Init()
        {
            this.Id = 0;
            this.StoreId = 0;
            this.CustomerId = string.Empty;
            this.LastUpdatedUtc = DateTime.UtcNow;
            this.ProductId = string.Empty;
            this.Quantity = 1;
            this.SelectionData = new Catalog.OptionSelectionList();
        }
        public WishListItem()
        {
            Init();
        }

        public Catalog.Product GetAssociatedProduct(MerchantTribeApplication app)
        {
            return app.CatalogServices.Products.Find(this.ProductId);
        }        

        //DTO
        public WishListItemDTO ToDto()
        {
            WishListItemDTO dto = new WishListItemDTO();
            dto.Id = this.Id;
            dto.StoreId = this.StoreId;
            dto.CustomerId = this.CustomerId;
            dto.LastUpdatedUtc = this.LastUpdatedUtc;
            dto.ProductId = this.ProductId ?? string.Empty;
            dto.Quantity = this.Quantity;
            foreach (Catalog.OptionSelection op in this.SelectionData)
            {
                dto.SelectionData.Add(op.ToDto());
            }
            return dto;
        }
        public void FromDto(WishListItemDTO dto)
        {
            if (dto == null) return;

            this.Id = dto.Id;
            this.StoreId = dto.StoreId;
            this.CustomerId = dto.CustomerId;
            this.LastUpdatedUtc = dto.LastUpdatedUtc;
            this.ProductId = dto.ProductId ?? string.Empty;
            this.Quantity = dto.Quantity;
            this.SelectionData.Clear();
            if (dto.SelectionData != null)
            {
                foreach (OptionSelectionDTO op in dto.SelectionData)
                {
                    Catalog.OptionSelection o = new Catalog.OptionSelection();
                    o.FromDto(op);
                    this.SelectionData.Add(o);
                }
            }
        }

    }

}

