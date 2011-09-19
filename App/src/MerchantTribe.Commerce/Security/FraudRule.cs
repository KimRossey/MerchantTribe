using System;
using System.Data;
using System.Collections.ObjectModel;

namespace MerchantTribe.Commerce.Security
{
	public class FraudRule
	{
        public string Bvin { get; set; }
        public DateTime LastUpdated { get; set; }
		public FraudRuleType RuleType {get;set;}
		public string RuleValue {get;set;}
        public long StoreId { get; set; }

        public FraudRule()
        {
            this.Bvin = string.Empty;
            this.LastUpdated = DateTime.UtcNow;
            this.RuleType = FraudRuleType.None;
            this.RuleValue = string.Empty;
            this.StoreId = 0;
        }        
	
	}
}
