using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Shipping
{
    public class ShippingServiceMessage
    {
        public ShippingServiceMessageType MessageType { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }

        public ShippingServiceMessage()
        {
            MessageType = ShippingServiceMessageType.Information;
            Code = string.Empty;
            Description = string.Empty;
        }

        public void SetError(string code, string description)
        {
            MessageType = ShippingServiceMessageType.Error;
            Code = code;
            Description = description;
        }

        public void SetDiagnostics(string code, string description)
        {
            MessageType = ShippingServiceMessageType.Diagnostics;
            Code = code;
            Description = description;
        }

        public void SetInfo(string code, string description)
        {
            MessageType = ShippingServiceMessageType.Information;
            Code = code;
            Description = description;
        }

    }
}
