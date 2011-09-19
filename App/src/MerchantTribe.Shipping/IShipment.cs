using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Web.Geography;

namespace MerchantTribe.Shipping
{
    public interface IShipment
    {
        List<IShippable> Items { get; set; }
        IAddress SourceAddress { get; set; }
        IAddress DestinationAddress { get; set; }        
    }
}
