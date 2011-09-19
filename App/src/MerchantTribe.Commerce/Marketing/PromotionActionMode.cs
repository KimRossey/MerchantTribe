using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Marketing
{
    public enum PromotionActionMode
    {
        Unknown = 0,
        ForLineItems = 1,
        ForSubTotal = 2,
        ForShipping = 3
    }
}
