using System;
using System.Data;
using System.Collections.ObjectModel;

namespace MerchantTribe.Commerce.Content
{
	public class PolicyBlock
	{
        public string Bvin { get; set; }
        public long StoreId { get; set; }
        public DateTime LastUpdated { get; set; }
		public string Name {get;set;}
		public string Description {get;set;}
		public string DescriptionPreTransform {get;set;}
		public int SortOrder {get;set;}
		public string PolicyID {get;set;}

        public PolicyBlock()
        {
            this.Bvin = string.Empty;
            this.StoreId = 0;
            this.LastUpdated = DateTime.UtcNow;
            this.Name = string.Empty;
            this.Description = string.Empty;
            this.DescriptionPreTransform = string.Empty;
            this.SortOrder = 0;
            this.PolicyID = string.Empty;
        }       
	}
}
