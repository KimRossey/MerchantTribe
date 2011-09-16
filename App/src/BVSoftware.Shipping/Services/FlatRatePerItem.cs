﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BVSoftware.Shipping.Services
{
    public class FlatRatePerItem: IShippingService
    {
        private List<ShippingServiceMessage> _Messages = new List<ShippingServiceMessage>();

        public string Name
        {
            get { return "Flat Rate Per Item";}
        }

        public string Id
        {
            get { return "3D6623E7-1E2C-444d-B860-A8F542133093"; }
		}

        public FlatRatePerItemSettings Settings { get; set; }
        public ServiceSettings BaseSettings
        {
            get { return Settings; }
        }

        public FlatRatePerItem()
        {
            Settings = new FlatRatePerItemSettings();
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

            decimal perItemAmount = Settings.Amount;

            ShippingRate r = new ShippingRate();
            r.ServiceId = this.Id;
            r.EstimatedCost = perItemAmount * totalItems;

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
