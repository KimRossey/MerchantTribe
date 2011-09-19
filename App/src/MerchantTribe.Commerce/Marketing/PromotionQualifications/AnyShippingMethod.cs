using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Marketing.PromotionQualifications
{
    public class AnyShippingMethod: PromotionQualificationBase
    {
        public override Guid TypeId
        {
            get { return new Guid("{" + PromotionQualificationBase.TypeIdAnyShippingMethod + "}"); }
        }

        public AnyShippingMethod()
            : base()
        {
            this.ProcessingCost = RelativeProcessingCost.Lowest;            
        }

        public override string FriendlyDescription(MerchantTribeApplication app)
        {
            return "When Any Shipping Method";
        }

        public override bool MeetsQualification(PromotionContext context)
        {
            if (context == null) return false;
            if (context.Order == null) return false;
            if (context.Mode == PromotionType.Sale) return false;

            return true;
        }
    }
}