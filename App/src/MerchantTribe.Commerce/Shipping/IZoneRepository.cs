using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Shipping
{
    public interface IZoneRepository
    {
        List<Zone> GetZones(long storeId);
    }
}
