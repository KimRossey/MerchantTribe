using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Shipping.Services
{
    public class FlatRatePerOrder: IShippingService
    {
        private List<ShippingServiceMessage> _Messages = new List<ShippingServiceMessage>();

        public string Name
        {
            get { return "Flat Rate Per Order";}
        }

        public string Id
        {
            get { return "301AA2B8-F43C-42fe-B77E-A7E1CB1DD40E"; }
		}

        public FlatRatePerOrderSettings Settings { get; set; }
        public ServiceSettings BaseSettings
        {
            get { return Settings; }
        }

        public FlatRatePerOrder()
        {
            Settings = new FlatRatePerOrderSettings();
        }

        public List<IServiceCode> ListAllServiceCodes()
        {
            return new List<IServiceCode>();
        }

        public List<IShippingRate> RateShipment(IShipment shipment)
        {                        
            ShippingRate r = new ShippingRate();
            r.ServiceId = this.Id;
            r.EstimatedCost = Settings.Amount;

            List<IShippingRate> rates = new List<IShippingRate>();
            rates.Add(r);
            
            return rates;
        }

        public List<ShippingServiceMessage> LatestMessages
        {
            get
            {
                return _Messages;
            }
            set
            {
                _Messages = value;
            }
        }
    }
}
