using System;
using System.Xml;
using System.Xml.XPath;
using System.Collections.ObjectModel;
using System.Data;

namespace MerchantTribe.Commerce.Metrics
{	
	public class SearchQuery
	{      
        public string Bvin {get;set;}
        public long StoreId { get; set; }
        public DateTime LastUpdated {get;set;}        
		public string ShopperID {get;set;}
		public string QueryPhrase {get;set;}
		
        private void clear()
        {
            this.Bvin = string.Empty;
            this.StoreId = 0;
            this.LastUpdated = DateTime.UtcNow;
            this.ShopperID = string.Empty;
            this.QueryPhrase = string.Empty;
        }
		public SearchQuery()
		{
            clear();            
		}
		public SearchQuery(string userId, string searchPhrase)
		{
            clear();
			ShopperID = userId;
			QueryPhrase = searchPhrase;
		}

	}


}
