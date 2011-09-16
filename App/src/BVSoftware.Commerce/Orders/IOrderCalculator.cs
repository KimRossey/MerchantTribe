using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BVSoftware.Commerce.Orders
{
    public interface IOrderCalculator
    {
        bool Calculate(Order o);
    }
}
