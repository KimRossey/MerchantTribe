using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Shipping.Services
{
    public class RateTableByTotalWeight: IShippingService
    {

        private List<ShippingServiceMessage> _Messages = new List<ShippingServiceMessage>();

        public string Name
        {
            get { return "Rate Table By Total Weight";}
        }

        public string Id
        {
            get { return "06C22589-14A7-470f-88EC-AF559D040A7A"; }
		}

        public RateTableSettings Settings { get; set; }
        public ServiceSettings BaseSettings
        {
            get { return Settings; }
        }

        public RateTableByTotalWeight()
        {
            Settings = new RateTableSettings();
        }

        public List<IServiceCode> ListAllServiceCodes()
        {
            return new List<IServiceCode>();
        }

        public List<IShippingRate> RateShipment(IShipment shipment)
        {
            decimal totalWeight = 0;

            foreach (IShippable item in shipment.Items)
            {
                totalWeight += item.BoxWeight;
            }

            decimal amount = 0;
            if (Settings != null)
            {
                if (Settings.GetLevels() != null)
                {
                    amount = RateTableLevel.FindRateFromLevels(totalWeight, Settings.GetLevels());
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
