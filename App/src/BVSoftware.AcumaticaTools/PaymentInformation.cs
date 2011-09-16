using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BVSoftware.Payment;

namespace BVSoftware.AcumaticaTools
{
    public class PaymentInformation
    {                                
        public Decimal Amount { get; set; }
        public ActionType Action { get; set; }
        public CardData Card { get; set; }                       
        public string TransactionNumber { get; set; }
        public string AuthorizationCode { get; set; }
        public string BVPaymentMethodId { get; set; }

        public Dictionary<string, string> AdditionalSettings { get; set; }

        public PaymentInformation()
        {
            Amount = 0;
            Action = ActionType.CreditCardHold;
            Card = new CardData();
            TransactionNumber = string.Empty;
            AuthorizationCode = string.Empty;
            BVPaymentMethodId = string.Empty;
        }

    }
}
