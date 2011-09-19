using System;
using System.Collections.Generic;

namespace MerchantTribe.Commerce.Security
{	
	public class FraudCheckData
	{

		private string _IpAddress = string.Empty;
		private string _DomainName = string.Empty;
		private string _EmailAddress = string.Empty;
		private string _PhoneNumber = string.Empty;
		private string _CreditCard = string.Empty;
		
		public string IpAddress {
			get { return _IpAddress; }
			set { _IpAddress = value.Trim(); }
		}
		public string DomainName {
			get { return _DomainName; }
			set { _DomainName = value.Trim().ToLower(); }
		}
		public string EmailAddress {
			get { return _EmailAddress; }
			set { _EmailAddress = value.Trim().ToLower(); }
		}
		public string PhoneNumber {
			get { return _PhoneNumber; }
			set { _PhoneNumber = Utilities.CreditCardValidator.CleanCardNumber(value.Trim().ToLower()); }
		}
		public string CreditCard {
			get { return _CreditCard; }
			set { _CreditCard = Utilities.CreditCardValidator.CleanCardNumber(value.Trim().ToLower()); }
		}

        public List<string> Messages { get; set; }

        public FraudCheckData()
        {
            Messages = new List<string>();
        }
	}
}

