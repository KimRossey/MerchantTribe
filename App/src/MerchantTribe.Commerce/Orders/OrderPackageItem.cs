using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.CommerceDTO.v1.Orders;

namespace MerchantTribe.Commerce.Orders
{
    public class OrderPackageItem
    {
		public string ProductBvin {get;set;}
		public long LineItemId {get;set;}
		public int Quantity {get;set;}

        public OrderPackageItem()
		{
            this.ProductBvin = string.Empty;
            this.LineItemId = 0;
            this.Quantity = 0;            
		}

		public OrderPackageItem(string bvin, long itemId, int qty)
		{
            this.ProductBvin = bvin;
			this.LineItemId = itemId;
			this.Quantity = qty;
		}

        //DTO
        public OrderPackageItemDTO ToDto()
        {
            OrderPackageItemDTO dto = new OrderPackageItemDTO();

            dto.LineItemId = this.LineItemId;
            dto.ProductBvin = this.ProductBvin ?? string.Empty;
            dto.Quantity = this.Quantity;

            return dto;
        }
        public void FromDto(OrderPackageItemDTO dto)
        {
            if (dto == null) return;

            this.LineItemId = dto.LineItemId;
            this.ProductBvin = dto.ProductBvin ?? string.Empty;
            this.Quantity = dto.Quantity;
        }
    }
}
