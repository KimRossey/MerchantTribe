using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Commerce.Catalog;

namespace MerchantTribe.Commerce.Content.Templates.TagHandlers
{
    public class Area : ITagHandler
    {

        public Area()
        {
        }

        public string TagName
        {
            get { return "sys:area"; }
        }

        public string Process(MerchantTribeApplication app, Dictionary<string, ITagHandler> handlers, ParsedTag tag, string contents)
        {
            string areaName = tag.GetSafeAttribute("name");

            string result = contents;

            // Get area data from the category if it exists, otherwise use the default area content
            if (app.CurrentRequestContext.CurrentCategory != null)
            {
                CategoryPageVersion v = app.CurrentRequestContext.CurrentCategory.GetCurrentVersion();
                if (v != null)
                {
                    string fromCat = v.Areas.GetAreaContent(areaName);
                    if (fromCat.Trim().Length > 0)
                    {
                        result = fromCat;
                    }
                }
            }

            // do replacements for legacy tags here
            result = MerchantTribe.Commerce.Utilities.TagReplacer.ReplaceContentTags(result,
                            app,
                            "",
                            app.CurrentRequestContext.RoutingContext.HttpContext.Request.IsSecureConnection);
            return result;
        }
    }
}
