using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Content.Templates.TagHandlers
{
    public class TempMessages : ITagHandler
    {

        public string TagName
        {
            get { return "sys:tempmessages"; }
        }

        public string Process(MerchantTribeApplication app, Dictionary<string, ITagHandler> handlers, ParsedTag tag, string contents)
        {
            StringBuilder sb = new StringBuilder();

            foreach (string s in app.CurrentRequestContext.TempMessages)
            {
                sb.Append(s);
            }

            return sb.ToString();
        }
    }
}
