using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Shipping
{
    public interface IShippable
    {
        decimal BoxValue { get; set; }
        int QuantityOfItemsInBox { get; set; }

        WeightType BoxWeightType { get; set; }
        decimal BoxWeight { get; set; }
        
        LengthType BoxLengthType { get; set; }
        decimal BoxLength { get; set; }
        decimal BoxWidth { get; set; }
        decimal BoxHeight { get; set; }

        IShippable CloneShippable();
    }
}
