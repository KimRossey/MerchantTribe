using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Data;
using System.Net.Mail;
using System.Text;

namespace MerchantTribe.Commerce.Content
{	
	public class HtmlTemplate
	{
        public long Id { get; set; }
        public long StoreId { get; set; }
        public DateTime LastUpdatedUtc { get; set; }
		public string DisplayName {get;set;}
		public string From {get;set;}
		public string Subject {get;set;}
		public string Body {get;set;}		
		public string RepeatingSection {get;set;}
        public HtmlTemplateType TemplateType { get; set; }

        public HtmlTemplate()
        {
            this.Id = 0;
            this.StoreId = 0;
            this.LastUpdatedUtc = DateTime.UtcNow;
            this.From = string.Empty;
            this.DisplayName = string.Empty;
            this.Subject = string.Empty;
            this.Body = string.Empty;
            this.RepeatingSection = string.Empty;
            this.TemplateType = HtmlTemplateType.Custom;
        }

        public HtmlTemplate Clone()
        {
            HtmlTemplate t = new HtmlTemplate();
            t.Body = this.Body;
            t.DisplayName = this.DisplayName;
            t.From = this.From;
            t.LastUpdatedUtc = this.LastUpdatedUtc;
            t.RepeatingSection = this.RepeatingSection;
            t.StoreId = this.StoreId;
            t.Subject = this.Subject;
            t.TemplateType = HtmlTemplateType.Custom;
            return t;
        }
        public HtmlTemplate ReplaceTagsInTemplate(MerchantTribeApplication app, IReplaceable item)
        {
            List<IReplaceable> items = new List<IReplaceable>();
            items.Add(item);
            return ReplaceTagsInTemplate(app, items);
        }
        public HtmlTemplate ReplaceTagsInTemplate(MerchantTribeApplication app, IReplaceable item, List<IReplaceable> repeatingItems)
        {
            List<IReplaceable> items = new List<IReplaceable>();
            items.Add(item);
            return ReplaceTagsInTemplate(app, items, repeatingItems);
        }
        public HtmlTemplate ReplaceTagsInTemplate(MerchantTribeApplication app, List<IReplaceable> items)
        {
            return ReplaceTagsInTemplate(app, items, new List<IReplaceable>());
        }
        public HtmlTemplate ReplaceTagsInTemplate(MerchantTribeApplication app, List<IReplaceable> items, List<IReplaceable> repeatingItems)
        {
            HtmlTemplate copy = this.Clone();            

            // Replace Store Defaults
            foreach (HtmlTemplateTag tag in this.DefaultReplacementTags(app))
            {
                copy.Subject = tag.ReplaceTags(copy.Subject);
                copy.Body = tag.ReplaceTags(copy.Body);
                copy.From = tag.ReplaceTags(copy.From);
            }

            // Replace Tags in Body and Subject
            foreach (IReplaceable item in items)
            {
                foreach (HtmlTemplateTag tag in item.GetReplaceableTags(app))
                {
                    copy.Subject = tag.ReplaceTags(copy.Subject);
                    copy.Body = tag.ReplaceTags(copy.Body);
                    copy.From = tag.ReplaceTags(copy.From);
                }
            }

            // Build Repeating Section
            StringBuilder sb = new StringBuilder();
            foreach (IReplaceable repeatingItem in repeatingItems)
            {
                string temp = copy.RepeatingSection;
                foreach (HtmlTemplateTag tag in repeatingItem.GetReplaceableTags(app))
                {                
                    temp = tag.ReplaceTags(temp);                 
                }
                sb.Append(temp);
            }

            // Copy repeating section to body
            string allrepeating = sb.ToString();
            copy.Body = copy.Body.Replace("[[RepeatingSection]]", allrepeating);

            return copy;
        }

        public System.Net.Mail.MailMessage ConvertToMailMessage(string toEmail)
        {
            System.Net.Mail.MailMessage result = new System.Net.Mail.MailMessage(this.From, toEmail);
            result.IsBodyHtml = true;
            result.Body = this.Body;
            result.Subject = this.Subject;
            return result;
        }

        public List<HtmlTemplateTag> DefaultReplacementTags(MerchantTribeApplication app)
        {
            List<HtmlTemplateTag> result = new List<HtmlTemplateTag>();

            Accounts.Store currentStore = app.CurrentStore; 
            result.Add(new HtmlTemplateTag("[[Store.Address]]", app.ContactServices.Addresses.FindStoreContactAddress().ToHtmlString()));
            result.Add(new HtmlTemplateTag("[[Store.ContactEmail]]", currentStore.Settings.MailServer.EmailForGeneral));                                
            result.Add(new HtmlTemplateTag("[[Store.Logo]]", Utilities.HtmlRendering.Logo(currentStore, false)));
            result.Add(new HtmlTemplateTag("[[Store.SecureUrl]]", currentStore.RootUrlSecure()));
            result.Add(new HtmlTemplateTag("[[Store.StoreName]]", currentStore.StoreName));
            result.Add(new HtmlTemplateTag("[[Store.StandardUrl]]", currentStore.RootUrl()));
            result.Add(new HtmlTemplateTag("[[Store.CurrentLocalTime]]", DateTime.Now.ToString()));
            result.Add(new HtmlTemplateTag("[[Store.CurrentUtcTime]]", DateTime.UtcNow.ToString()));
            
            return result;
        }

	}
}

