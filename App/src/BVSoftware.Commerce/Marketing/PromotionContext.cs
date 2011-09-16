using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BVSoftware.Commerce.Marketing
{
    public class PromotionContext
    {
        public PromotionType Mode { get; set; }
        public BVApplication BVApp { get; set; }        
        public Orders.Order Order { get; set; }
        public Catalog.Product Product { get; set; }
        public Catalog.UserSpecificPrice UserPrice { get; set; }
        public DateTime CurrentDateAndTimeUtc { get; set; }
        public string CustomerDescription { get; set; }
        public Membership.CustomerAccount CurrentCustomer { get; set; }

        public PromotionContext(BVApplication bvapp, 
                                Catalog.Product p, 
                                Catalog.UserSpecificPrice up, 
                                Membership.CustomerAccount currentUser, 
                                DateTime currentTimeUtc)
        {
            this.CustomerDescription = string.Empty;
            this.Mode = PromotionType.Sale;
            this.BVApp = bvapp;
            this.Order = null;
            this.Product = p;
            this.UserPrice = up;
            this.CurrentDateAndTimeUtc = DateTime.UtcNow;
            this.CurrentCustomer = currentUser;
        }

        public PromotionContext(BVApplication bvapp,
                                Orders.Order o,
                                Membership.CustomerAccount currentUser,
                                DateTime currentTimeUtc)
        {
            this.CustomerDescription = string.Empty;
            this.Mode = PromotionType.Offer;
            this.BVApp = bvapp;
            this.Order = o;
            this.Product = null;
            this.UserPrice = null;
            this.CurrentDateAndTimeUtc = DateTime.UtcNow;
            this.CurrentCustomer = currentUser;
        }


    }
}
