﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;

namespace MerchantTribe.Commerce.Utilities
{
    public class TagReplacer
    {
        public static string ReplaceContentTags(string source, MerchantTribeApplication app, string itemCount, bool isSecureRequest)            
        {
            Accounts.Store currentStore = app.CurrentStore;
            string currentUserId = SessionManager.GetCurrentUserId(app.CurrentStore);

            string output = source;
            
            RouteCollection r = System.Web.Routing.RouteTable.Routes;
            //VirtualPathData homeLink = r.GetVirtualPath(requestContext.RoutingContext, "homepage", new RouteValueDictionary());
            
            output = output.Replace("{{homelink}}", app.StoreUrl(isSecureRequest, false));
            output = output.Replace("{{logo}}", HtmlRendering.Logo(app, isSecureRequest));
            output = output.Replace("{{logotext}}", HtmlRendering.LogoText(app));
            output = output.Replace("{{headermenu}}", HtmlRendering.HeaderMenu(app.CurrentRequestContext.RoutingContext, app.CurrentRequestContext));
            output = output.Replace("{{cartlink}}", HtmlRendering.CartLink(app, itemCount));
            output = output.Replace("{{copyright}}", "<span class=\"copyright\">Copyright &copy;" + DateTime.Now.Year.ToString() + "</span>");
            output = output.Replace("{{headerlinks}}", HtmlRendering.HeaderLinks(app, currentUserId));
            output = output.Replace("{{searchform}}", HtmlRendering.SearchForm(app));
            output = output.Replace("{{assets}}", MerchantTribe.Commerce.Storage.DiskStorage.BaseUrlForStoreTheme(app, currentStore.Settings.ThemeId, isSecureRequest) + "assets/");            

            output = output.Replace("{{storeaddress}}", app.ContactServices.Addresses.FindStoreContactAddress().ToHtmlString());

            return output;
        }

    }
}
