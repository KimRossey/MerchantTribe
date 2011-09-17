using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Payment.ManualMethods
{
    public class PurchaseOrder: Method
    {
        public override string Name
        {
            get { return "Purchase Order"; }
        }

        public override string Id
        {
            get { return "PO"; }
        }

        public override void ProcessTransaction(Transaction t)
        {
            switch (t.Action)
            {
                case ActionType.PurchaseOrderInfo:
                    t.Result.Succeeded = true;
                    t.Result.ResponseCode = "OK";
                    t.Result.ResponseCodeDescription = "Purchase Order Info";
                    break;
                case ActionType.PurchaseOrderAccepted:
                    t.Result.ResponseCode = "OK";
                    t.Result.ResponseCodeDescription = "Purchase Order Accepted";
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
