using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Content.Templates.TagHandlers
{
    public class Copyright : ITagHandler
    {

        public string TagName
        {
            get { return "sys:copyright"; }
        }

        public string Process(MerchantTribeApplication app, Dictionary<string, ITagHandler> handlers, ParsedTag tag, string contents)
        {
            string by = contents;
            string byParam = tag.GetSafeAttribute("by");
            if (byParam.Trim().Length > 0) by = byParam;

            string result = "<span class=\"copyright\">Copyright &copy; ";
            result += DateTime.Now.Year.ToString();
            result += "</span>";
            if (by.Trim().Length > 0)
            {
                result += by;
            }
            return result;
        }
    }
}
