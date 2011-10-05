using System;
using System.Xml;
using System.Xml.Serialization;
using System.Web;
using System.ComponentModel;
using System.Data;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MerchantTribe.Web.Cryptography;
using MerchantTribe.Web;
using MerchantTribe.Commerce.Contacts;
using MerchantTribe.CommerceDTO.v1.Membership;
using MerchantTribe.CommerceDTO.v1.Contacts;
using System.Linq;

namespace MerchantTribe.Commerce.Membership
{    
	public class CustomerAccount: Content.IReplaceable
	{
        
        // Base info
        public string Bvin {get;set;}
        public long StoreId { get; set; }
        public string Email {get;set;}
        public string FirstName {get;set;}
        public string LastName {get;set;}
        public string Password {get;set;}
        public string Salt {get;set;}
        public Address ShippingAddress { get; set; }
        public Address BillingAddress { get; set; }

        // Addresses        
        private AddressList _Addresses = new Contacts.AddressList();
        private PhoneNumberList _Phones = new Contacts.PhoneNumberList();        
        public AddressList Addresses
        {
            get { return _Addresses; }
            set { _Addresses = value; }
        }        
        public PhoneNumberList Phones
        {
            get { return _Phones; }
            set { _Phones = value; }
        }

        // Other
        public bool TaxExempt {get;set;}
        public string Notes {get;set;}
        public string PricingGroupId {get;set;}

        // Security
        public bool Locked {get;set;}
        public DateTime LockedUntilUtc {get;set;}
        public int FailedLoginCount {get;set;}

        // Tracking
        public DateTime LastUpdatedUtc {get;set;}
		public System.DateTime CreationDateUtc {get;set;}
		public System.DateTime LastLoginDateUtc {get;set;}
	        
		// Constructor			
		public CustomerAccount()
		{
            this.Bvin = string.Empty;
            this.StoreId = 0;            
            this.Email = string.Empty;
            this.FirstName = string.Empty;
            this.LastName = string.Empty;
            this.Password = string.Empty;
            this.Salt = string.Empty;

            this.TaxExempt = false;
            this.Notes = string.Empty;
            this.PricingGroupId = string.Empty;

            this.Locked = false;
            this.LockedUntilUtc = DateTime.UtcNow;
            this.FailedLoginCount = 0;

            this.LastUpdatedUtc = DateTime.UtcNow;
            this.CreationDateUtc = DateTime.UtcNow;
            this.LastLoginDateUtc = DateTime.UtcNow;
            this.BillingAddress = new Address();
            this.ShippingAddress = new Address();
		}	
	    
        public List<Content.HtmlTemplateTag> GetReplaceableTags(MerchantTribeApplication app)
        {
            List<Content.HtmlTemplateTag> result = new List<Content.HtmlTemplateTag>();

            result.Add(new Content.HtmlTemplateTag("[[User.Bvin]]", this.Bvin));
            result.Add(new Content.HtmlTemplateTag("[[User.Comment]]", this.Notes));
            result.Add(new Content.HtmlTemplateTag("[[User.Notes]]", this.Notes));
            result.Add(new Content.HtmlTemplateTag("[[User.CreationDate]]", this.CreationDateUtc.ToLocalTime().ToString()));
            result.Add(new Content.HtmlTemplateTag("[[User.Email]]", this.Email));
            result.Add(new Content.HtmlTemplateTag("[[User.UserName]]", this.Email)); // Legacy template tag
            result.Add(new Content.HtmlTemplateTag("[[User.FirstName]]", this.FirstName));
            result.Add(new Content.HtmlTemplateTag("[[User.LastLoginDate]]", this.LastLoginDateUtc.ToLocalTime().ToString()));
            result.Add(new Content.HtmlTemplateTag("[[User.LastName]]", this.LastName));
            result.Add(new Content.HtmlTemplateTag("[[User.LastUpdated]]", this.LastUpdatedUtc.ToLocalTime().ToString()));
            result.Add(new Content.HtmlTemplateTag("[[User.Locked]]", this.Locked.ToString()));
            result.Add(new Content.HtmlTemplateTag("[[User.LockedUntil]]", this.LockedUntilUtc.ToLocalTime().ToString()));
            result.Add(new Content.HtmlTemplateTag("[[User.Password]]", this.Password));

            return result;
        }
	
        public bool CheckIfNewAddressAndAddNoUpdate(Contacts.Address address)
        {
            bool addressFound = false;
            foreach (Contacts.Address currAddress in this.Addresses)
            {
                if (currAddress.IsEqualTo(address))
                {
                    addressFound = true;
                    break;
                }
            }

            bool createdAddress = false;

            if (!addressFound)
            {
                address.Bvin = System.Guid.NewGuid().ToString();
                this._Addresses.Add(address);
                createdAddress = true;
            }

            return createdAddress;
        }
        public bool DeleteAddress(string bvin)
        {
            bool result = false;

            int index = -1;
            for (int i = 0; i < _Addresses.Count;i++)
            {
                if (_Addresses[i].Bvin == bvin)
                {
                    index = i;
                    break;
                }
            }
            if (index >= 0)
            {
                _Addresses.RemoveAt(index);
                return true;
            }

            return result;
        }
        public bool UpdateAddress(Contacts.Address a)
        {
            bool result = false;

            int index = -1;
            for (int i = 0; i < _Addresses.Count; i++)
            {
                if (_Addresses[i].Bvin == a.Bvin)
                {
                    index = i;
                    break;
                }
            }
            if (index >= 0)
            {
                _Addresses[index] = a;
                return true;
            }

            return result;
        }
        public string EncryptPassword(string password)
        {
            string result = string.Empty;
            result = Hashing.Md5Hash(password, this.Salt);
            return result;
        }               

        //DTO
        public CustomerAccountDTO ToDto()
        {
            CustomerAccountDTO dto = new CustomerAccountDTO();

            dto.Bvin = this.Bvin;
            dto.Email = this.Email;
            dto.FirstName = this.FirstName;
            dto.LastName = this.LastName;
            dto.Password = this.Password;
            dto.Salt = this.Salt;

            dto.TaxExempt = this.TaxExempt;
            dto.Notes = this.Notes;
            dto.PricingGroupId = this.PricingGroupId;

            dto.FailedLoginCount = this.FailedLoginCount;

            dto.LastUpdatedUtc = this.LastUpdatedUtc;
            dto.CreationDateUtc = this.CreationDateUtc;
            dto.LastLoginDateUtc = this.LastLoginDateUtc;

            foreach (Contacts.Address a in this.Addresses)
            {
                dto.Addresses.Add(a.ToDto());
            }

            dto.ShippingAddress = this.ShippingAddress.ToDto();
            dto.BillingAddress = this.BillingAddress.ToDto();
            return dto;
        }
        public void FromDto(CustomerAccountDTO dto)
        {
            this.Bvin = dto.Bvin;
            this.Email = dto.Email;
            this.FirstName = dto.FirstName;
            this.LastName = dto.LastName;
            this.Password = dto.Password;
            this.Salt = dto.Salt;

            this.TaxExempt = dto.TaxExempt;
            this.Notes = dto.Notes;
            this.PricingGroupId = dto.PricingGroupId;

            this.FailedLoginCount = dto.FailedLoginCount;

            this.LastUpdatedUtc = dto.LastUpdatedUtc;
            this.CreationDateUtc = dto.CreationDateUtc;
            this.LastLoginDateUtc = dto.LastLoginDateUtc;

            foreach (AddressDTO a in dto.Addresses)
            {
                Contacts.Address addr = new Contacts.Address();
                addr.FromDto(a);
                this.Addresses.Add(addr);
            }

            this.ShippingAddress.FromDto(dto.ShippingAddress);
            this.BillingAddress.FromDto(dto.BillingAddress);
        }
							                
    }
}
