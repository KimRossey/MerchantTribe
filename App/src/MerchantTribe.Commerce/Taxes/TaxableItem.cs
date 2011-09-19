using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Taxes
{
    public class TaxableItem : ITaxable
    {
        public decimal Value { get; set; }
        public long ScheduleId { get; set; }
        public decimal ShippingValue { get; set; }
        public decimal TaxedValue { get; set; }

        public TaxableItem()
        {
            Value = 0M;
            ScheduleId = 0;
            ShippingValue = 0M;
            TaxedValue = 0M;
        }

        #region ITaxable Members

        public decimal TaxableValue()
        {
            return Value;
        }

        public long TaxScheduleId()
        {
            return ScheduleId;
        }

        public decimal TaxableShippingValue()
        {
            return ShippingValue;
        }

        public void SetTaxValue(decimal calculatedTax)
        {
            TaxedValue = calculatedTax;
        }

        public void IncrementTaxValue(decimal calculatedTax)
        {
            this.TaxedValue += calculatedTax;
        }

        public void ClearTaxValue()
        {
            this.TaxedValue = 0M;
        }

        #endregion

    }
}
