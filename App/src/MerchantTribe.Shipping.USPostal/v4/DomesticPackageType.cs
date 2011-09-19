using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Shipping.USPostal.v4
{
    public enum DomesticPackageType
    {
        Ignore = -1,
        Variable = 0,
        FlatRateBox = 1,
        FlatRateBoxSmall = 2,
        FlatRateBoxMedium = 3,
        FlatRateBoxLarge = 4,
        FlatRateEnvelope = 5,
        FlatRateEnvelopePadded = 50,
        FlatRateEnvelopeLegal = 51,
        FlatRateEnvelopeSmall = 52,
        FlatRateEnvelopeWindow = 53,
        FlatRateEnvelopeGiftCard = 54,
        RegionalBoxRateA = 200,
        RegionalBoxRateB = 201,
        Rectangular = 6,
        NonRectangular = 7,
        FirstClassLetter = 100,
        FirstClassFlat = 101,
        FirstClassParcel = 102,
        FirstClassPostCard = 103,
        
    }
}
