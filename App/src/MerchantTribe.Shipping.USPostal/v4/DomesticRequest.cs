using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Shipping.USPostal.v4
{
    public class DomesticRequest
    {
        public string ApiUrl { get; set; }
        public string UserId { get; set; }
        public string Revision { get; set; }
        public List<DomesticPackage> Packages { get; set; }

        public DomesticRequest()
        {
            ApiUrl = USPostalConstants.ApiUrl;
            UserId = USPostalConstants.ApiUsername;
            Revision = "2";
            this.Packages = new List<DomesticPackage>();
        }
    }
}
