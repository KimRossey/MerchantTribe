using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Shipping
{
    public class Shippable: IShippable
    {

        public decimal BoxValue {get;set;}
        public int QuantityOfItemsInBox {get;set;}
        public WeightType BoxWeightType {get;set;}
        public decimal BoxWeight {get;set;}
        public LengthType BoxLengthType {get;set;}
        public decimal BoxLength {get;set;}
        public decimal BoxWidth {get;set;}
        public decimal BoxHeight {get;set;}

        public Shippable()
        {
            BoxValue = 0;
            QuantityOfItemsInBox = 0;
            BoxWeightType = WeightType.Pounds;
            BoxWeight = 0;
            BoxLengthType = LengthType.Inches;
            BoxLength = 0;
            BoxWidth = 0;
            BoxHeight = 0;
        }


        public IShippable CloneShippable()
        {
            Shippable clone = new Shippable();
            clone.BoxValue = this.BoxValue;
            clone.QuantityOfItemsInBox = this.QuantityOfItemsInBox;
            clone.BoxWeightType = this.BoxWeightType;
            clone.BoxWeight = this.BoxWeight;
            clone.BoxLengthType = this.BoxLengthType;
            clone.BoxLength = this.BoxLength;
            clone.BoxWidth = this.BoxWidth;
            clone.BoxHeight = this.BoxHeight;
            return clone;
        }
    }
}
