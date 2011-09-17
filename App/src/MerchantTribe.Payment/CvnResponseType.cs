using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Payment
{
    public enum CvnResponseType
    {        
        Match,
        NoMatch,
        Unavailable,
        Error
    }
}
