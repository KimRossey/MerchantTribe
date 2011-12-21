using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Content.Templates.TagHandlers
{
    public class AdminPanel : ITagHandler
    {
        public string TagName
        {
            get { return "sys:adminpanel"; }
        }

        public string Process(MerchantTribeApplication app, Dictionary<string, ITagHandler> handlers, ParsedTag tag, string contents)
        {

            StringBuilder sb = new StringBuilder();

            if (app.CurrentRequestContext.IsAdmin(app))
            {

               string logourl = System.Web.VirtualPathUtility.ToAbsolute("~/images/system/AdminPanelLogo.png");

               sb.Append("<div id=\"adminpanel\">");
               sb.Append("<a id=\"adminpanellogo\" href=\"");
               sb.Append(app.CurrentStore.RootUrlSecure());
               sb.Append("bvadmin\"><img src=\"" + logourl + "\" alt=\"MerchantTribeStore\" /></a>");                            
       
               if (app.CurrentStore.Settings.StoreClosed == true)
               {
                    sb.Append("<a href=\"");
                   sb.Append(app.CurrentStore.RootUrlSecure());
                   sb.Append("bvadmin/configuration/general.aspx\" class=\"red\">");              
                   sb.Append("*** STORE IS CLOSED, SHOPPERS CAN'T SEE THIS PAGE ***</a>");
               }


               if (app.CurrentRequestContext.CurrentCategory != null)
               {
                   MerchantTribe.Commerce.Catalog.Category c = app.CurrentRequestContext.CurrentCategory;
                   string editUrl = app.CatalogServices.EditorRouteForCategory(c.SourceType, c.Bvin);
                   editUrl = app.CurrentRelativeRoot + editUrl.TrimStart('/');
                   sb.Append("<a href=\"");
                   sb.Append(editUrl);
                   sb.Append("\" class=\"editlink\">Edit This Page</a>");
               }

               sb.Append("<a href=\"" + app.CurrentStore.RootUrlSecure() + "bvadmin\" class=\"right\">Go To Admin Dashboard</a>");
               sb.Append("</div>");
                
            }

            return sb.ToString();
        }        
    }
}
