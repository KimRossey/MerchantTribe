using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Shipping.USPostal.v4
{
    public enum InternationalServiceType
    {
        All = -1,

        FirstClassInternationalLetter = 13,
        FirstClassInternationalFlats = 14,
        FirstClassInternationalParcel = 15,

        ExpressMailInternational = 1,
        ExpressMailInternationalFlatRateEnvelope = 10,
        ExpressMailInternationalLegalFlatRateEnvelope = 17,

        PriorityMailInternational = 2,
        PriorityMailInternationalFlatRateEnvelope = 8,
        PriorityMailInternationalMediumFlatRateBox = 9,
        PriorityMailInternationalLargeFlatRateBox = 11,
        PriorityMailInternationalGiftCardFlatRate = 18,
        PriorityMailInternationalWindowFlatRateEnvelope = 19,
        PriorityMailInternationalSmallFlatRateEnvelope = 20,
        PriorityMailInternationalLegalFlatRateEnvelope = 22,
        PriorityMailInternationalPaddedFlatRateEnvelope = 23,

        GlobalExpressGuaranteed = 4,    
        GlobalExpressGuaranteedRectangular = 6,
        GlobalExpressGuaranteedNonRectangular = 7,
        GlobalExpressGuaranteedEnvelopes = 12,

        AirmailParcelPost = 8888,
        AirmailLetter = 9999                        
    }
}
