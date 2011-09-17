using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Payment.ManualMethods
{
    public class Cash: Method
    {

        public override string Name
        {
            get { return "Cash"; }
        }

        public override string Id
        {
            get { return "CASH"; }
        }

        public override void ProcessTransaction(Transaction t)
        {
            switch (t.Action)
            {
                case ActionType.CashReceived:
                    t.Result.Succeeded = true;
                    t.Result.ResponseCode = "OK";
                    t.Result.ResponseCodeDescription = "Cash Received";
                    break;
                case ActionType.CashReturned:
                    t.Result.ResponseCode = "OK";
                    t.Result.ResponseCodeDescription = "Cash Returned";
                    t.Result.Succeeded = true;
                    break;
                default:
                    t.Result.Succeeded = false;
                    t.Result.Messages.Add(new Message("Operation Not Supported by this Method", "OPFAIL", MessageType.Error));
                    break;
            }
        }

        public override MethodSettings BaseSettings
        {
            get { return new MethodSettings(); }
        }
    }
}
