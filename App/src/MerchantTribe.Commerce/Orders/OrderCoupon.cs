using System;
using System.Data;
using System.Collections.ObjectModel;
using MerchantTribe.CommerceDTO.v1.Orders;

namespace MerchantTribe.Commerce.Orders
{

	public class OrderCoupon : IEquatable<OrderCoupon>
	{
        public long Id { get; set; }
        public long StoreId { get; set; }
        public DateTime LastUpdatedUtc { get; set; }
		public string OrderBvin {get;set;}
		public string CouponCode {get;set;}
		public bool IsUsed {get;set;}
        public string UserId { get; set; }

		public OrderCoupon()
		{
            this.Id = 0;
            this.StoreId = 0;
            this.LastUpdatedUtc = DateTime.UtcNow;
            this.OrderBvin = string.Empty;
            this.CouponCode = string.Empty;
            this.IsUsed = false;
            this.UserId = string.Empty;
		}

        bool System.IEquatable<OrderCoupon>.Equals(OrderCoupon other)
        {
            return this.Id == other.Id;
        }
			
		
        //DTO
        public OrderCouponDTO ToDto()
        {
            OrderCouponDTO dto = new OrderCouponDTO();

            dto.Id = this.Id;
            dto.StoreId = this.StoreId;
            dto.LastUpdatedUtc = this.LastUpdatedUtc;
            dto.OrderBvin = this.OrderBvin;
            dto.CouponCode = this.CouponCode;
            dto.IsUsed = this.IsUsed;
            dto.UserId = this.UserId;

            return dto;
        }
        public void FromDto(OrderCouponDTO dto)
        {
            if (dto == null) return;

            this.Id = dto.Id;
            this.StoreId = dto.StoreId;
            this.LastUpdatedUtc = dto.LastUpdatedUtc;
            this.OrderBvin = dto.OrderBvin ?? string.Empty;
            this.CouponCode = dto.CouponCode ?? string.Empty;
            this.IsUsed = dto.IsUsed;
            this.UserId = dto.UserId ?? string.Empty;

        }
	}

}