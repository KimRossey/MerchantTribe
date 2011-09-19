using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Payment;

namespace MerchantTribe.Billing
{
    public class BillingAccount
    {
        public long Id { get; set; }
        private string _Email = string.Empty;
        public string Email 
        {
            get { return _Email; }
            set { _Email = value.Trim().ToLowerInvariant(); }
        }
        public CardData CreditCard { get; set; }
        public string BillingZipCode { get; set; }

        public BillingAccount()
        {
            Id = 0;
            Email = string.Empty;
            CreditCard = new CardData();
            BillingZipCode = string.Empty;
        }

        public bool HasValidCreditCard(DateTime localTime)
        {
            return CreditCard.IsCardValid(localTime);            
        }
    }
}
