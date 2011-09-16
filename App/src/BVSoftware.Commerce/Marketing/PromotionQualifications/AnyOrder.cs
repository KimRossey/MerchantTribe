using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BVSoftware.Commerce.Marketing.PromotionQualifications
{
    public class AnyOrder: PromotionQualificationBase
    {
        public override Guid TypeId
        {
            get { return new Guid("{" + PromotionQualificationBase.TypeIdAnyOrder + "}"); }
        }

        public AnyOrder():base()
        {
            this.ProcessingCost = RelativeProcessingCost.Lowest;            
        }

        public override string FriendlyDescription(BVApplication bvapp)
        {
            return "When Any Order";
        }

        public override bool MeetsQualification(PromotionContext context)
        {
            if (context == null) return false;
            if (context.Order == null) return false;

            return true;
        }
    }
}
