using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Xml.Serialization;
using MerchantTribe.CommerceDTO.v1.Orders;

namespace MerchantTribe.Commerce.Orders
{
	public class OrderNote
	{
        public long Id { get; set; }
        public long StoreId { get; set; }
        public DateTime LastUpdatedUtc { get; set; }
		public string OrderID {get;set;}
		public DateTime AuditDate {get;set;}
		public string Note {get;set;}
		public bool IsPublic {get;set;}

		public OrderNote()
		{
            this.Id = 0;
            this.StoreId = 0;
            this.LastUpdatedUtc = DateTime.UtcNow;
            this.OrderID = string.Empty;
            this.AuditDate = DateTime.UtcNow;
            this.Note = string.Empty;
            this.IsPublic = false;
		}

        //DTO
        public OrderNoteDTO ToDto()
        {
            OrderNoteDTO dto = new OrderNoteDTO();

            dto.AuditDate = this.AuditDate;
            dto.Id = this.Id;
            dto.IsPublic = this.IsPublic;
            dto.LastUpdatedUtc = this.LastUpdatedUtc;
            dto.Note = this.Note ?? string.Empty;
            dto.OrderID = this.OrderID ?? string.Empty;
            dto.StoreId = this.StoreId;
            
            return dto;
        }
        public void FromDto(OrderNoteDTO dto)
        {
            if (dto == null) return;

            this.AuditDate = dto.AuditDate;
            this.Id = dto.Id;
            this.IsPublic = dto.IsPublic;
            this.LastUpdatedUtc = dto.LastUpdatedUtc;
            this.Note = dto.Note ?? string.Empty;
            this.OrderID = dto.OrderID ?? string.Empty;
            this.StoreId = dto.StoreId;

        }
	}
}
