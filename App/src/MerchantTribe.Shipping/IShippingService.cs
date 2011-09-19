using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Shipping
{
    public interface IShippingService
    {
        string Name { get; }
        
        string Id { get; }        

        ServiceSettings BaseSettings { get; }

        List<IShippingRate> RateShipment(IShipment shipment);

        List<IServiceCode> ListAllServiceCodes();

        List<ShippingServiceMessage> LatestMessages { get; set; }
    }
}
