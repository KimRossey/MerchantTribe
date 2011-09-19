using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Shipping.USPostal.v4
{
    public class InternationalRequest
    {
            public string ApiUrl { get; set; }
            public string UserId { get; set; }
            public string Revision { get; set; }
            public List<InternationalPackage> Packages { get; set; }

            public InternationalRequest()
            {
                ApiUrl = USPostalConstants.ApiUrl;
                UserId = USPostalConstants.ApiUsername;
                Revision = "2";
                this.Packages = new List<InternationalPackage>();
            }
    }
}
