using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MerchantTribeStore.Areas.signup.Models
{
    public class FeatureSection
    {
        public string Name { get; set; }
        public List<FeatureDetails> Features { get; set; }

        public FeatureSection()
        {
            Name = string.Empty;
            Features = new List<FeatureDetails>();
        }
    }
}