using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Content
{
    public enum HtmlTemplateType
    {
        Custom = 0,
        NewOrder = 1,
        NewOrderForAdmin = 2,
        OrderUpdated = 3,
        OrderShipment = 4,
        ForgotPassword = 100,
        EmailFriend = 101,
        ContactFormToAdmin = 102,
        DropShippingNotice = 200        
    }
}
