using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Marketing.PromotionActions
{
    public interface IPromotionAction
    {
        long Id { get; set; }
        Guid TypeId { get; }
        Dictionary<string, string> Settings { get; set; }
        
        string FriendlyDescription { get; }

        bool ApplyAction(PromotionContext context, PromotionActionMode mode);
    }
}
