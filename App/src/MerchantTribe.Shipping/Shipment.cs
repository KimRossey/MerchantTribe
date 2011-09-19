using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Web.Geography;

namespace MerchantTribe.Shipping
{
    public class Shipment: IShipment
    {


        public List<IShippable> Items {get;set;}
        public IAddress SourceAddress {get;set;}
        public IAddress DestinationAddress {get;set;}

        public Shipment()
        {
            SourceAddress = new MerchantTribe.Web.Geography.SimpleAddress();
            DestinationAddress = new MerchantTribe.Web.Geography.SimpleAddress();
            Items = new List<IShippable>();            
        }

        public static Shipment CloneAddressesFromInterface(IShipment source)
        {
            Shipment result = new Shipment();
            result.DestinationAddress = MerchantTribe.Web.Geography.SimpleAddress.CloneAddress(source.DestinationAddress);
            result.SourceAddress = MerchantTribe.Web.Geography.SimpleAddress.CloneAddress(source.SourceAddress);
            return result;
        }

    }
}
