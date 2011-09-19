using System;
using System.Data;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace MerchantTribe.Commerce.Membership
{

	[Serializable()]
	public class AuthenticationToken
	{
        public string Bvin { get; set; }
        public DateTime LastUpdated { get; set; }		
		public System.DateTime ExpirationDate {
			get { return ExpirationDateUTC.ToLocalTime(); }
			set { ExpirationDateUTC = value.ToUniversalTime(); }
		}
		public System.DateTime ExpirationDateUTC {get;set;}
		public string UserBvin {get;set;}
		public bool TokenRejected {get;set;}

        public AuthenticationToken()
        {
            this.Bvin = string.Empty;
            this.LastUpdated = DateTime.UtcNow;
            this.ExpirationDateUTC = DateTime.MinValue.AddDays(1);
            this.UserBvin = string.Empty;
            this.TokenRejected = true;
        }

		public bool IsExpired {
			get {
				if (System.DateTime.Compare(DateTime.Now, ExpirationDate) <= 0) {
					return false;
				}
				else {
					return true;
				}
			}
		}
     	
	}
}
