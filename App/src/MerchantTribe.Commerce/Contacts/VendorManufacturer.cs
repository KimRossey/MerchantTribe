
using System;
using System.Data;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using MerchantTribe.CommerceDTO.v1.Contacts;

namespace MerchantTribe.Commerce.Contacts
{
	public class VendorManufacturer: Content.IReplaceable
	{
        public string Bvin { get; set; }
        public long StoreId { get; set; }
        public DateTime LastUpdated { get; set; }		
		public string DisplayName {get;set;}
		public string EmailAddress {get;set;}
		public Contacts.Address Address {get;set;}
		public string DropShipEmailTemplateId {get;set;}
        public List<VendorManufacturerContact> Contacts { get; set; }
        public VendorManufacturerType ContactType { get; set; }

        public VendorManufacturer()
        {
            this.Bvin = string.Empty;
            this.LastUpdated = DateTime.UtcNow;
            this.DisplayName = string.Empty;
            this.EmailAddress = string.Empty;
            this.Address = new Contacts.Address();
            this.DropShipEmailTemplateId = string.Empty;
            this.Contacts = new List<VendorManufacturerContact>();
            this.ContactType = VendorManufacturerType.Unknown;
        }

        public List<Content.HtmlTemplateTag> GetReplaceableTags(MerchantTribeApplication app)
        {
			List<Content.HtmlTemplateTag> result = new List<Content.HtmlTemplateTag>();
			result.Add(new Content.HtmlTemplateTag("[[VendorManufacturer.EmailAddress]]", this.EmailAddress));
			result.Add(new Content.HtmlTemplateTag("[[VendorManufacturer.Name]]", this.DisplayName));
			return result;
		}

        public bool AddContact(string userId)
        {
            if (!ContactExists(userId))
            {
                Contacts.Add(new VendorManufacturerContact() { StoreId = this.StoreId, UserId = userId, VendorManufacturerId = this.Bvin });
            }
            return true;
        }
        public bool RemoveContact(string userId)
        {
            var c = Contacts.Where(y => y.UserId == userId).SingleOrDefault();
            if (c != null)
            {
                Contacts.Remove(c);
            }
            return true;
        }
        public bool ContactExists(string userId)
        {
            var c = Contacts.Where(y => y.UserId == userId).SingleOrDefault();
            if (c != null) return true;
            return false;
        }

        //DTO
        public VendorManufacturerDTO ToDto()
        {
            VendorManufacturerDTO dto = new VendorManufacturerDTO();

            this.Bvin = string.Empty;
            this.LastUpdated = DateTime.UtcNow;
            this.DisplayName = string.Empty;
            this.EmailAddress = string.Empty;
            this.Address = new Contacts.Address();
            this.DropShipEmailTemplateId = string.Empty;
            this.Contacts = new List<VendorManufacturerContact>();
            this.ContactType = VendorManufacturerType.Unknown;

            return dto;
        }
        public void FromDto(VendorManufacturerDTO dto)
        {
            if (dto == null) return;

            this.Bvin = dto.Bvin;
            this.LastUpdated = dto.LastUpdated;
            this.DisplayName = dto.DisplayName;
            this.EmailAddress = dto.EmailAddress;
            this.Address.FromDto(dto.Address);
            this.DropShipEmailTemplateId = dto.DropShipEmailTemplateId;
            this.Contacts.Clear();
            foreach (VendorManufacturerContactDTO c in dto.Contacts)
            {
                VendorManufacturerContact v = new VendorManufacturerContact();
                v.FromDto(c);
                this.Contacts.Add(v);
            }
            this.ContactType = (VendorManufacturerType)((int)dto.ContactType);

        }
     
    }
}
