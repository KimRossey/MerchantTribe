using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Content;
using MerchantTribe.Commerce.Accounts;
using System.IO;

namespace MerchantTribeStore.Controllers
{
    public class StyleSheetsController : Controller
    {


        // Initialize Store Specific Request Data
        //MerchantTribe.Commerce.RequestContext _BVRequestContext = new RequestContext();
        public MerchantTribe.Commerce.RequestContext CurrentRequestContext
        {
            get { return MTApp.CurrentRequestContext; }
            set { MTApp.CurrentRequestContext = value; }
        }

        public MerchantTribeApplication MTApp { get; set; }

        public MerchantTribe.Commerce.Accounts.Store CurrentStore
        {
            get { return MTApp.CurrentStore; }
            set { MTApp.CurrentStore = value; }
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            MTApp = MerchantTribeApplication.InstantiateForDataBase(new RequestContext());

            // Store routing context for URL Rewriting            
            MTApp.CurrentRequestContext.RoutingContext = this.Request.RequestContext;

            // Determine store id        
            MTApp.CurrentStore = MerchantTribe.Commerce.Utilities.UrlHelper.ParseStoreFromUrl(System.Web.HttpContext.Current.Request.Url, MTApp.AccountServices);
            if (MTApp.CurrentStore == null)
            {
                Response.Redirect("~/storenotfound");
            }
            

        }        

        //
        // GET: /StyleSheets/
        //[OutputCache(VaryByHeader = "Host", VaryByParam = "themename", VaryByCustom = "disablecsscaching", Duration = 150)]
        public ActionResult Index(string themename)
        {
            string themeId = themename.Replace("theme-", "");
            ThemeManager themes = MTApp.ThemeManager();

            string css = string.Empty;
            string physicalFile = string.Empty;
            string baseUrl = Url.Content("~");

            switch (themeId.Trim().ToLowerInvariant())
            {
                case "admin":
                    physicalFile = Server.MapPath("~/bvadmin/Styles.css");
                    css = themes.AdminStyleSheetContentMinifiedAndReplaced(physicalFile, baseUrl);
                    break;
                case "system":
                    physicalFile = Server.MapPath("~/content/system.css");
                    css = themes.AdminStyleSheetContentMinifiedAndReplaced(physicalFile, baseUrl);
                    break;
                case "flexedit":
                    physicalFile = Server.MapPath("~/content/FlexEdit.css");
                    css = themes.AdminStyleSheetContentMinifiedAndReplaced(physicalFile, baseUrl);
                    break;
                default:
                    css = themes.CurrentStyleSheetContentMinifiedAndReplaced(themeId);
                    break;
            }
                                           
                                    
            Response.AddHeader("content-disposition", "attachment; filename=styles.css");            
            return Content(css, "text/css");                        
        }

    }
}
