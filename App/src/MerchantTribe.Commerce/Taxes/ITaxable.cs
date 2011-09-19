using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Taxes
{
    public interface ITaxable
    {
        decimal TaxableValue();
        long TaxScheduleId();
        decimal TaxableShippingValue();
        void IncrementTaxValue(decimal calculatedTax);
        void ClearTaxValue();
    }
}
