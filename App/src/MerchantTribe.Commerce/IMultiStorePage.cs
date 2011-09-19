using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Commerce.Accounts;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Taxes;
namespace MerchantTribe.Commerce
{
    public interface IMultiStorePage
    {
        MerchantTribeApplication MTApp { get; set; }        
    }
}
