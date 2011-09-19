using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Web;
using MerchantTribe.Billing;

namespace MerchantTribe.Commerce.Accounts
{
    public class CreateStoreException : Exception
    {
        public CreateStoreException(string message)
            : base(message)
        {

        }
        public CreateStoreException(string reason, Exception inner)
            : base(reason, inner)
        {

        }
    }
}
