using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Orders
{
    public interface IOrderCalculator
    {
        bool Calculate(Order o);
    }
}
