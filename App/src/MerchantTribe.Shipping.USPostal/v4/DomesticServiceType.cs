using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Shipping.USPostal.v4
{
    public enum DomesticServiceType
    {
        All = -1,
        Online = -2,
        FirstClass = 0,
        FirstClassHoldForPickupCommercial = 100,
        PriorityMail = 1,
        PriorityMailCommercial = 101,
        PriorityMailHoldForPickupCommercial = 102,
        ExpressMail = 2,
        ExpressMailCommerceial = 200,        
        ExpressMailSundayHoliday = 3,
        ExpressMailSundayHolidayCommercial = 203,
        ExpressMailHoldForPickup = 4,
        ExpressMailHoldForPickupCommercial = 204,
        ParcelPost = 6,
        MediaMail = 7,
        LibraryMaterial = 8                        
    }
}
