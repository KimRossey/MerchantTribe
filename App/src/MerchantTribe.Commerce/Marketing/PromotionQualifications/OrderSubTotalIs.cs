using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Marketing.PromotionQualifications
{
    public class OrderSubTotalIs: PromotionQualificationBase
    {
        public override Guid TypeId
        {
            get { return new Guid("{" + PromotionQualificationBase.TypeIdOrderSubTotalIs + "}"); }
        }
       
        public decimal Amount 
        {
            get {return GetSettingAsDecimal("Amount");}
            set {SetSetting("Amount", value);}
        }

        public override string FriendlyDescription(MerchantTribeApplication app)
        {
            string result = "When Order Sub Total >= " + this.Amount.ToString("C");            
            return result;
        }
        public OrderSubTotalIs()
            : base()
        {
            this.ProcessingCost = RelativeProcessingCost.Lower;            
        }
       
        public override bool MeetsQualification(PromotionContext context)
        {
            if (context == null) return false;
            if (context.Mode != PromotionType.Offer) return false;
            if (context.Order == null) return false;

            if (context.Order.TotalOrderAfterDiscounts >= this.Amount) return true;

            return false;

        }
    }
}