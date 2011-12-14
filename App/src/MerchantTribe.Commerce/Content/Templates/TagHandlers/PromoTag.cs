using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Content.Templates.TagHandlers
{
    public class PromoTag : ITagHandler
    {

        public string TagName
        {
            get { return "sys:promotag"; }
        }

        public string Process(MerchantTribeApplication app, Dictionary<string, ITagHandler> handlers, ParsedTag tag, string contents)
        {
            return MerchantTribe.Commerce.Utilities.HtmlRendering.PromoTag();
        }
    }
}
