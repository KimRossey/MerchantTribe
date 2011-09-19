using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Marketing.PromotionQualifications
{
    public class AnyProduct: PromotionQualificationBase
    {
        public override Guid TypeId
        {
            get { return new Guid("{" + PromotionQualificationBase.TypeIdAnyProduct + "}"); }
        }

        public AnyProduct():base()
        {
            this.ProcessingCost = RelativeProcessingCost.Lowest;            
        }

        public override string FriendlyDescription(MerchantTribeApplication app)
        {
            return "When Any Product";
        }

        public override bool MeetsQualification(PromotionContext context)
        {
            if (context == null) return false;
            if (context.Product == null) return false;
            if (context.UserPrice == null) return false;

            return true;
        }
    }
}
