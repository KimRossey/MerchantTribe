using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Membership;
using System.ComponentModel.DataAnnotations;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Utilities;

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

        public SingleProductViewModel(Product p, MerchantTribeApplication mtapp)
        {
            this.UserPrice = mtapp.PriceProduct(p, mtapp.CurrentCustomer, null);
            this.IsFirstItem = false;
            this.IsLastItem = false;
            this.Item = p;
            this.ImageUrl = MerchantTribe.Commerce.Storage.DiskStorage.ProductImageUrlSmall(
                mtapp.CurrentStore.Id,
                p.Bvin,
                p.ImageFileSmall,
                mtapp.CurrentRequestContext.RoutingContext.HttpContext.Request.IsSecureConnection);                
            this.ProductLink = UrlRewriter.BuildUrlForProduct(p,
                                            mtapp.CurrentRequestContext.RoutingContext,
                                            string.Empty);
        }
        
    }
}