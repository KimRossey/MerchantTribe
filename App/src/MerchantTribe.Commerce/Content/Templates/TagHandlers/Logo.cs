using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Content.Templates.TagHandlers
{
    public class Logo : ITagHandler
    {

        public string TagName
        {
            get { return "sys:logo"; }
        }

        public string Process(MerchantTribeApplication app, Dictionary<string, ITagHandler> handlers, ParsedTag tag, string contents)
        {
            StringBuilder sb = new StringBuilder();
            bool isSecureRequest = app.CurrentRequestContext.RoutingContext.HttpContext.Request.IsSecureConnection;
            bool textOnly = false;
            string textOnlyTag = tag.GetSafeAttribute("textonly").Trim().ToLowerInvariant();
            if (textOnlyTag == "1" || textOnlyTag == "y" || textOnlyTag == "yes" || textOnlyTag == "true") textOnly = true;

            string storeRootUrl = app.CurrentStore.RootUrl();
            string storeName = app.CurrentStore.Settings.FriendlyName;

            string logoImage = app.CurrentStore.Settings.LogoImageFullUrl(app, isSecureRequest);
            string logoText = app.CurrentStore.Settings.LogoText;

            sb.Append("<a href=\"" + storeRootUrl + "\" title=\"" + storeName + "\"");

            if (contents.Trim().Length > 0)
            {
                sb.Append(">");
                sb.Append(contents);
            }
            else if (app.CurrentStore.Settings.UseLogoImage && textOnly == false)
            {
                sb.Append("><img src=\"" + logoImage + "\" alt=\"" + storeName + "\" />");
            }
            else
            {
                sb.Append(" class=\"logo\">");
                sb.Append(System.Web.HttpUtility.HtmlEncode(logoText));
            }
            sb.Append("</a>");

            return sb.ToString();
        }
    }
}
