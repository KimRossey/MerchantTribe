using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Web.Geography;

namespace MerchantTribe.Commerce.Shipping
{
    public class ZoneArea
    {
        public string CountryIsoAlpha3 { get; set; }
        public string RegionAbbreviation { get; set; }

        public ZoneArea()
        {
        }

        public string CountryName
        {
            get
            {
                string result = CountryIsoAlpha3;
                Country c = Country.FindByISOCode(CountryIsoAlpha3);
                if (c != null)
                {
                    result = c.DisplayName;
                }
                return result;
            }
        }
    }
}
