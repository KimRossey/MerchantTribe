using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Marketing.PromotionActions
{
    public class OrderShippingAdjustment: PromotionActionBase
    {
        public override Guid TypeId
        {
            get { return new Guid("{" + PromotionActionBase.TypeIdOrderShippingAdjustment +"}"); }
        }
     
        public AmountTypes AdjustmentType {
            get { string temp = GetSetting("AdjustmentType");
                  AmountTypes result = AmountTypes.MonetaryAmount;
                  Enum.TryParse(temp, out result);
                  return result;
                }
            set { SetSetting("AdjustmentType", (int)value); }
        }
        public decimal Amount 
        {
            get { return GetSettingAsDecimal("Amount"); }
            set { SetSetting("Amount", value); } 
        }

        public OrderShippingAdjustment()
        {
            Id = 0;
            Settings = new Dictionary<string, string>();
            AdjustmentType = AmountTypes.MonetaryAmount;
            Amount = 0m;
        }
        public OrderShippingAdjustment(AmountTypes type, decimal amount)
        {
            Id = 0;
            Settings = new Dictionary<string, string>();
            this.AdjustmentType = type;
            this.Amount = amount;
        }

        public override string FriendlyDescription
        {
            get {
                bool isDiscount = this.Amount < 0;

                string result = (isDiscount ? "Decrease":"Increase") + " Order Shipping by ";
                    
                    switch (this.AdjustmentType)
                    {
                        case AmountTypes.MonetaryAmount:
                            result += Math.Abs(this.Amount).ToString("c");
                            break;
                        case AmountTypes.Percent:
                            result += (Math.Abs(this.Amount) / 100m).ToString("p");
                            break;
                    }
                    return result;
            }
        }

        public override bool ApplyAction(PromotionContext context, PromotionActionMode mode)
        {
            if (context == null) return false;
            if (context.Order == null) return false;
            
            // only apply when applying to shipping areas
            if (mode != PromotionActionMode.ForShipping) return false;

            decimal adjustment = 0m;

            switch(this.AdjustmentType)
            {
                case AmountTypes.MonetaryAmount:
                    adjustment = Utilities.Money.GetDiscountAmount(context.Order.TotalShippingBeforeDiscounts, this.Amount);
                    break;
                case AmountTypes.Percent:
                    adjustment = Utilities.Money.GetDiscountAmountByPercent(context.Order.TotalShippingBeforeDiscounts, this.Amount);
                    break;
            }

            context.Order.AddShippingDiscount(adjustment, context.CustomerDescription);
            
            return true;
        }


       
    }
}
