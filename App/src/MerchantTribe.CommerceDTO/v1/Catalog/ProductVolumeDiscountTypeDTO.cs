using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace MerchantTribe.CommerceDTO.v1.Catalog
{
    [DataContract]
    public enum ProductVolumeDiscountTypeDTO
    {
        [EnumMember]
        None = 0,
        [EnumMember]
        Percentage = 1,
        [EnumMember]
        Amount = 2
    }
}
