using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Marketing
{
    public class PromotionContext
    {
        public PromotionType Mode { get; set; }
        public MerchantTribeApplication MTApp { get; set; }        
        public Orders.Order Order { get; set; }
        public Catalog.Product Product { get; set; }
        public Catalog.UserSpecificPrice UserPrice { get; set; }
        public DateTime CurrentDateAndTimeUtc { get; set; }
        public string CustomerDescription { get; set; }
        public Membership.CustomerAccount CurrentCustomer { get; set; }

        public PromotionContext(MerchantTribeApplication app, 
                                Catalog.Product p, 
                                Catalog.UserSpecificPrice up, 
                                Membership.CustomerAccount currentUser, 
                                DateTime currentTimeUtc)
        {
            this.CustomerDescription = string.Empty;
            this.Mode = PromotionType.Sale;
            this.MTApp = app;
            this.Order = null;
            this.Product = p;
            this.UserPrice = up;
            this.CurrentDateAndTimeUtc = DateTime.UtcNow;
            this.CurrentCustomer = currentUser;
        }

        public PromotionContext(MerchantTribeApplication app,
                                Orders.Order o,
                                Membership.CustomerAccount currentUser,
                                DateTime currentTimeUtc)
        {
            this.CustomerDescription = string.Empty;
            this.Mode = PromotionType.Offer;
            this.MTApp = app;
            this.Order = o;
            this.Product = null;
            this.UserPrice = null;
            this.CurrentDateAndTimeUtc = DateTime.UtcNow;
            this.CurrentCustomer = currentUser;
        }


    }
}
