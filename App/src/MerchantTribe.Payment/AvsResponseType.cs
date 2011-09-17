using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Payment
{
    public enum AvsResponseType
    {
        Unavailable,            // B, G, P, R, S, U
        Error,                  // E
        FullMatch,              // X, Y
        NoMatch,                // N
        PartialMatchPostalCode, // W, Z
        PartialMatchAddress     // A
    }

}
