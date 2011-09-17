using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Payment.ManualMethods
{
    public class Check: Method
    {
        public override string Name
        {
            get { return "Check"; }
        }

        public override string Id
        {
            get { return "CHECK"; }
        }

        public override void ProcessTransaction(Transaction t)
        {
            switch (t.Action)
            {
                case ActionType.CheckReceived:
                    t.Result.Succeeded = true;
                    t.Result.ResponseCode = "OK";
                    t.Result.ResponseCodeDescription = "Check Received";
                    break;
                case ActionType.CheckReturned:
                    t.Result.ResponseCode = "OK";
                    t.Result.ResponseCodeDescription = "Check Returned";
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
