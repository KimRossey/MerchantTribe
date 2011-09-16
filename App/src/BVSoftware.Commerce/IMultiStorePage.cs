using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BVSoftware.Commerce.Accounts;
using BVSoftware.Commerce.Catalog;
using BVSoftware.Commerce.Taxes;
namespace BVSoftware.Commerce
{
    public interface IMultiStorePage
    {
        BVApplication BVApp { get; set; }        
    }
}
