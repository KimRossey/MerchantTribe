using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Content.Templates.TagHandlers
{
    public class MainMenu : ITagHandler
    {

        public string TagName
        {
            get { return "sys:mainmenu"; }
        }

        public string Process(MerchantTribeApplication app, Dictionary<string, ITagHandler> handlers, ParsedTag tag, string contents)
        {
            StringBuilder sb = new StringBuilder();

            int linksPerRow = 9;
            string tryLinksPerRow = tag.GetSafeAttribute("linksperrow");
            int temp1 = -1;
            if (int.TryParse(tryLinksPerRow, out temp1)) linksPerRow = temp1;
            if (linksPerRow < 1) linksPerRow = 1;

            int maxLinks = 9;
            int temp2 = -1;
            string tryMaxLinks = tag.GetSafeAttribute("maxlinks");
            if (int.TryParse(tryMaxLinks, out temp2)) maxLinks = temp2;
            if (maxLinks < 1) maxLinks = 1;

            int tabIndex = 0;
            string tryTabIndex = tag.GetSafeAttribute("tabindex");
            int.TryParse(tryTabIndex, out tabIndex);
            if (tabIndex < 0) tabIndex = 0;

            int tempTabIndex = 0;

            //Find Categories to Display in Menu
            List<Catalog.CategorySnapshot> categories = app.CatalogServices.Categories.FindForMainMenu();

            // Limit number of links
            int stopCount = categories.Count - 1;
            if ((maxLinks > 0) && ((maxLinks - 1) < stopCount))
            {
                stopCount = (maxLinks - 1);
            }


            //Open List
            if (categories.Count > 0)
            {
                sb.Append("<ul>");
            }

            tempTabIndex = tabIndex;

            //Build each Row
            for (int i = 0; i <= stopCount; i++)
            {
                sb.Append(BuildLink(categories[i], app.CurrentRequestContext.RoutingContext, ref tempTabIndex));
                // Move to Next Row if Not Last Item
                int endOfRowCount = (i + 1) % linksPerRow;

                if ((endOfRowCount == 0) && (i < stopCount))
                {
                    sb.Append("</ul><ul>");
                }
            }

            // Close List
            if (categories.Count > 0)
            {
                sb.Append("</ul>");
            }

            return sb.ToString();
        }

        private string BuildLink(Catalog.CategorySnapshot c, System.Web.Routing.RequestContext routingContext, ref int tempTabIndex)
        {
            bool displayActiveTab = true;
            string result = string.Empty;

            StringBuilder sbLink = new StringBuilder();

            sbLink.Append("<li");

            if ((c.Bvin == SessionManager.CategoryLastId) && displayActiveTab)
            {
                sbLink.Append(" class=\"activemainmenuitem\" >");
            }
            else
            {
                sbLink.Append(">");
            }

            sbLink.Append("<a href=\"");
            sbLink.Append(Utilities.UrlRewriter.BuildUrlForCategory(c, routingContext));

            if (c.CustomPageOpenInNewWindow)
            {
                sbLink.Append("\" target=\"_blank\"");
            }
            else
            {
                sbLink.Append("\"");
            }

            if (tempTabIndex != -1)
            {
                sbLink.Append(" TabIndex=\"" + tempTabIndex.ToString() + "\" ");
                tempTabIndex += 1;
            }

            sbLink.Append(" class=\"actuator\" title=\"" + c.MetaTitle + "\">");
            sbLink.Append("<span>" + c.Name + "</span>");
            sbLink.Append("</a></li>");

            result = sbLink.ToString();
            return result;
        }
    }
}
