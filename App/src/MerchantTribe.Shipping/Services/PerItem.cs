using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Shipping.Services
{
    public class PerItem: IShippingService
    {
        private List<ShippingServiceMessage> _Messages = new List<ShippingServiceMessage>();

        public string Name
        {
            get { return "Rate Table Per Item";}
        }

        public string Id
        {
            get { return "41B590A7-003C-48d1-8446-EAE93C156AA1"; }
		}

        public RateTableSettings Settings { get; set; }
        public ServiceSettings BaseSettings
        {
            get { return Settings; }
        }

        public PerItem()
        {
            Settings = new RateTableSettings();
        }

        public List<IServiceCode> ListAllServiceCodes()
        {
            return new List<IServiceCode>();
        }

        public List<IShippingRate> RateShipment(IShipment shipment)
        {
            int totalItems = 0;

            foreach (IShippable item in shipment.Items)
            {
                totalItems += item.QuantityOfItemsInBox;
            }


            decimal amountPerItem = 0;
            if (Settings != null)
            {
                if (Settings.GetLevels() != null)
                {
                    amountPerItem = RateTableLevel.FindRateFromLevels(totalItems, Settings.GetLevels());
                }
            }

            ShippingRate r = new ShippingRate();
            r.ServiceId = this.Id;
            r.EstimatedCost = amountPerItem * totalItems;

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
