using System;
using System.Data;
using System.Collections.ObjectModel;

namespace MerchantTribe.Commerce.Contacts
{
	public class AffiliateReferralSearchCriteria
	{
		public long AffiliateId {get;set;}
		public DateTime? StartDateUtc {get;set;}
		public DateTime? EndDateUtc {get;set;}

        public AffiliateReferralSearchCriteria()
        {
            this.AffiliateId = 0;
            this.StartDateUtc = null;
            this.EndDateUtc = null;
        }
	}
}
