using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Marketing.PromotionQualifications
{
    public interface IPromotionQualification
    {
        long Id { get; set; }
        Guid TypeId { get; }
        RelativeProcessingCost ProcessingCost { get; set; }
        Dictionary<string, string> Settings { get; set; }

        string FriendlyDescription(MerchantTribeApplication app);

        bool MeetsQualification(PromotionContext context);
    }
}
