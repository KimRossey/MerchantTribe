using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Content
{
    public interface IReplaceable
    {
        List<HtmlTemplateTag> GetReplaceableTags(MerchantTribeApplication app);
    }
}
