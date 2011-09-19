using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace MerchantTribe.CommerceDTO.v1.Orders
{
    [DataContract]
    public enum OrderShippingStatusDTO
    {
        [EnumMember]
        Unknown = 0,
        [EnumMember]
        Unshipped = 1,
        [EnumMember]
        PartiallyShipped = 2,
        [EnumMember]
        FullyShipped = 3,
        [EnumMember]
        NonShipping = 4
    }
}
