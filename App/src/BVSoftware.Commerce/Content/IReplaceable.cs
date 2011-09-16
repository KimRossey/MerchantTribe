using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BVSoftware.Commerce.Content
{
    public interface IReplaceable
    {
        List<HtmlTemplateTag> GetReplaceableTags(BVApplication bvapp);
    }
}
