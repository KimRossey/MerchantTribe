using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Payment.Methods
{
    public class TestGateway: Method
    {
        // Properties
        public override string Name
        {
            get { return "Test Gateway"; }
        }
        public override string Id
        {
            get { return "FCACE46F-7B9C-4b49-82B6-426CF522C0C6"; }
        }
        
        public TestGatewaySettings Settings { get; set; }
        public override MethodSettings BaseSettings
        {
            get
            {
                return Settings;
            }
        }
        
        // Constructor
        public TestGateway()
        {
            Settings = new TestGatewaySettings();
        }

        // Methods
        public override void ProcessTransaction(Transaction t)
        {
            bool result = false;

            switch (t.Action)
            {
                case ActionType.CreditCardCapture:
                    result = Settings.ResponseForCapture;
                    break;
                case ActionType.CreditCardCharge:
                    result = Settings.ResponseForCharge;
                    break;
                case ActionType.CreditCardHold:
                    result = Settings.ResponseForHold;
                    break;
                case ActionType.CreditCardRefund:
                    result = Settings.ResponseForRefund;
                    break;
                case ActionType.CreditCardVoid:
                    result = Settings.ResponseForVoid;
                    break;
            }

            t.Result.ReferenceNumber = System.Guid.NewGuid().ToString();
            t.Result.ReferenceNumber2 = System.Guid.NewGuid().ToString();

            t.Result.Succeeded = result;

        }

        
    }
}
