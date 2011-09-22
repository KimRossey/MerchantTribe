using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Membership;

namespace MerchantTribeStore.Models
{
    public class SingleProductViewModel
    {
        public Product Item { get; set; }
        public bool IsFirstItem { get; set; }
        public bool IsLastItem { get; set; }
        public UserSpecificPrice UserPrice { get; set; }
        public string ImageUrl { get; set; }
        public string ProductLink { get; set; }

        public SingleProductViewModel()
        {
            Item = null;
            IsFirstItem = false;
            IsLastItem = false;
            UserPrice = null;
            ImageUrl = string.Empty;
            ProductLink = string.Empty;
        }
        
    }
}