using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Content.Templates.TagHandlers
{
    public class SearchForm : ITagHandler
    {
        public string TagName
        {
            get { return "sys:searchform"; }
        }

        public string Process(MerchantTribeApplication app, Dictionary<string, ITagHandler> handlers, ParsedTag tag, string contents)
        {            
            StringBuilder sb = new StringBuilder();
            
            string rootUrl = app.StoreUrl(false, true);
            string buttonUrl = app.ThemeManager().ButtonUrl("Go", app.CurrentRequestContext.RoutingContext.HttpContext.Request.IsSecureConnection);
            sb.Append("<form class=\"searchform\" action=\"" + rootUrl + "search\" method=\"get\">");
            sb.Append("<input type=\"text\" name=\"q\" class=\"searchinput\" /> <input class=\"searchgo\" type=\"image\" src=\"" + buttonUrl + "\" alt=\"Search\" />");
            sb.Append("</form>");

            return sb.ToString();                                                
        }
    }
}
