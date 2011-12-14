using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Content.Templates.TagHandlers
{
    public class Include : ITagHandler
    {
        public string TagName
        {
            get { return "sys:include"; }
        }

        public string Process(MerchantTribeApplication app, Dictionary<string, ITagHandler> handlers, ParsedTag tag, string contents)
        {
            string partName = tag.GetSafeAttribute("part");

            ThemeManager tm = app.ThemeManager();
            string result = tm.GetTemplatePartFromCurrentTheme(partName);
            TemplateProcessor proc = new TemplateProcessor(app, result, handlers);
            string processed = proc.RenderForDisplay();
            return processed;
        }

        public Include()
        {
        }
    }
}
