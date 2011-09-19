﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Web;

namespace MerchantTribe.Commerce
{
    public class SiteMapGenerator
    {
        public static string BuildForStore(BVApplication app)
        {
            if (app == null) return string.Empty;

            string root = app.CurrentStore.RootUrl();
            SiteMapNode rootNode = new SiteMapNode();
            
            // home
            rootNode.AddUrl(root);
            // sitemap
            rootNode.AddUrl(root + "sitemap");

            // Categories
            foreach (Catalog.CategorySnapshot cat in app.CatalogServices.Categories.FindAll())
            {                
                string caturl = Utilities.UrlRewriter.BuildUrlForCategory(cat, app.CurrentRequestContext.RoutingContext);
                rootNode.AddUrl(root.TrimEnd('/') + caturl);
            }

            // Products
            foreach (Catalog.Product p in app.CatalogServices.Products.FindAllPaged(1,3000))
            {
                string produrl = Utilities.UrlRewriter.BuildUrlForProduct(p, app.CurrentRequestContext.RoutingContext, string.Empty);
                rootNode.AddUrl(root.TrimEnd('/') + produrl);
            }
            return rootNode.RenderAsXmlSiteMap();                        
        }
    }
}
