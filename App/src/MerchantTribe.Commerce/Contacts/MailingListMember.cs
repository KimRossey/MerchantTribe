using System;
using System.Data;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Xml;

namespace MerchantTribe.Commerce.Contacts
{
	public class MailingListMember: Content.IReplaceable
	{
        public long Id { get; set; }
        public long ListId { get; set; }
        public DateTime LastUpdatedUtc { get; set; }		
		public string EmailAddress {get;set;}
		public string FirstName {get;set;}
		public string LastName {get;set;}
        public long StoreId { get; set; }

		public MailingListMember()
		{
            this.Id = 0;
            this.LastUpdatedUtc = DateTime.UtcNow;
            this.ListId = 0;
            this.EmailAddress = string.Empty;
            this.FirstName = string.Empty;
            this.LastName = string.Empty;
            this.StoreId = 0;
		}

        public List<Content.HtmlTemplateTag> GetReplaceableTags(MerchantTribeApplication app)
        {
            List<Content.HtmlTemplateTag> result = new List<Content.HtmlTemplateTag>();

            result.Add(new Content.HtmlTemplateTag("[[MailingListMember.EmailAddress]]", this.EmailAddress));
            result.Add(new Content.HtmlTemplateTag("[[MailingListMember.FirstName]]", this.FirstName));
            result.Add(new Content.HtmlTemplateTag("[[MailingListMember.LastName]]", this.LastName));

            return result;
        }
    }
}

