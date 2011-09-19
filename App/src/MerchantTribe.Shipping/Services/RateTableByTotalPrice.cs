using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Shipping.Services
{
    public class RateTableByTotalPrice: IShippingService
    {
        private List<ShippingServiceMessage> _Messages = new List<ShippingServiceMessage>();

        public string Name
        {
            get { return "Rate Table By Total Price";}
        }

        public string Id
        {
            get { return "9F896073-EE1F-400c-8B54-D9858B06AA01"; }
		}

        public RateTableSettings Settings { get; set; }
        public ServiceSettings BaseSettings
        {
            get { return Settings; }
        }

        public RateTableByTotalPrice()
        {
            Settings = new RateTableSettings();
        }

        public List<IServiceCode> ListAllServiceCodes()
        {
            return new List<IServiceCode>();
        }

        public List<IShippingRate> RateShipment(IShipment shipment)
        {
            decimal totalValue = 0;

            foreach (IShippable item in shipment.Items)
            {
                totalValue += item.BoxValue;
            }


            decimal amount = 0;
            if (Settings != null)
            {
                if (Settings.GetLevels() != null)
                {
                    amount = RateTableLevel.FindRateFromLevels(totalValue, Settings.GetLevels());
                }
            }

            ShippingRate r = new ShippingRate();
            r.ServiceId = this.Id;
            r.EstimatedCost = amount;

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
