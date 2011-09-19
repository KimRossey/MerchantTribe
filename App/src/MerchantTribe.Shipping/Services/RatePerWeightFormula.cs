using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Shipping.Services
{
    public class RatePerWeightFormula: IShippingService
    {
        private List<ShippingServiceMessage> _Messages = new List<ShippingServiceMessage>();

        public string Name
        {
            get { return "Rate Per Weight Formula";}
        }

        public string Id
        {
            get { return "5AAF9016-B03F-4e7c-8596-193F5EFFFDC3"; }
		}

        public RatePerWeightFormulaSettings Settings { get; set; }
        public ServiceSettings BaseSettings
        {
            get { return Settings; }
        }

        public RatePerWeightFormula()
        {
            Settings = new RatePerWeightFormulaSettings();
        }

        public List<IServiceCode> ListAllServiceCodes()
        {
            return new List<IServiceCode>();
        }

        public List<IShippingRate> RateShipment(IShipment shipment)
        {
            List<IShippingRate> rates = new List<IShippingRate>();

            // Total Up Weight
            decimal totalWeight = 0;
            foreach (IShippable item in shipment.Items)
            {                
                totalWeight += item.BoxWeight;
            }

            // Check for max weight
            if (totalWeight > Settings.MaxWeight || totalWeight < Settings.MinWeight)
            {
                return rates;
            }

            // Calculate Overage
            decimal extraWeight = 0;
            if (totalWeight > Settings.BaseWeight)
            {
                extraWeight = totalWeight - Settings.BaseWeight;
            }            
            int extraWeightWhole = (int)Math.Ceiling(extraWeight);

            // Base + Overage Charges
            decimal theRate = Settings.BaseAmount + (extraWeightWhole * Settings.AdditionalWeightCharge);
            
            ShippingRate r = new ShippingRate();
            r.ServiceId = this.Id;
            r.EstimatedCost = theRate;

            
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
