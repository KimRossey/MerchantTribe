using System;
using System.Data;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace MerchantTribe.Commerce.Catalog
{
    [Serializable]
	public class ProductPropertyValue
	{		
		public ProductPropertyValue()
		{
            this.Id = 0;
            this.StoreId = 0;
            this.ProductID = string.Empty;
		    this.PropertyID = -1;
		    this.StringValue = string.Empty;            
		}
        public long Id { get; set; }
        public long StoreId { get; set; }
		public string ProductID {get;set;}
		public long PropertyID { get;set;}
		public string StringValue {get;set;}
        public long StringValueAsLong()
        {
            long result = -1;
            long.TryParse(this.StringValue, out result);
            return result;
        }

	}

}
