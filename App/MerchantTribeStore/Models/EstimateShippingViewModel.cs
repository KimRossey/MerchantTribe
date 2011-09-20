using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Shipping;

namespace MerchantTribeStore.Models
{
    public class EstimateShippingViewModel
    {
        public string CountryId { get; set; }
        public string RegionId { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }

        public List<ShippingRateDisplay> Rates { get; set; }

        public EstimateShippingViewModel()
        {
            CountryId = MerchantTribe.Web.Geography.Country.UnitedStatesCountryBvin;
            RegionId = string.Empty;
            PostalCode = string.Empty;
            City = string.Empty;

            Rates = new List<ShippingRateDisplay>();
        }
    }
}