using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Orders
{
    public interface IPurchasable
    {
        PurchasableSnapshot AsPurchasable(Catalog.OptionSelectionList selectionData, MerchantTribeApplication app);
        PurchasableSnapshot AsPurchasable(Catalog.OptionSelectionList selectionData, MerchantTribeApplication app, bool calculateUserPrice);
    }
}
