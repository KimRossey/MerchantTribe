using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace MerchantTribe.CommerceDTO.v1.Contacts
{
    [DataContract]
    public enum AffiliateCommissionTypeDTO
    {
        [EnumMember]
        None = 0,
        [EnumMember]
        PercentageCommission = 1,
        [EnumMember]
        FlatRateCommission = 2
    }
}
