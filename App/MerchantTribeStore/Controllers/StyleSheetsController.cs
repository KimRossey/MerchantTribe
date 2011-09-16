using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BVSoftware.Commerce;
using BVSoftware.Commerce.Content;
using BVSoftware.Commerce.Accounts;
using System.IO;

namespace BVCommerce.Controllers
{
    public class StyleSheetsController : Controller
    {


        // Initialize Store Specific Request Data
        //BVSoftware.Commerce.RequestContext _BVRequestContext = new RequestContext();
        public BVSoftware.Commerce.RequestContext CurrentRequestContext
        {
            get { return BVApp.CurrentRequestContext; }
            set { BVApp.CurrentRequestContext = value; }
        }

        public BVApplication BVApp { get; set; }

        public BVSoftware.Commerce.Accounts.Store CurrentStore
        {
            get { return BVApp.CurrentStore; }
            set { BVApp.CurrentStore = value; }
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            BVApp = BVApplication.InstantiateForDataBase(new RequestContext());

            // Store routing context for URL Rewriting            
            BVApp.CurrentRequestContext.RoutingContext = this.Request.RequestContext;

            // Determine store id        
            BVApp.CurrentStore = BVSoftware.Commerce.Utilities.UrlHelper.ParseStoreFromUrl(System.Web.HttpContext.Current.Request.Url, BVApp.AccountServices);
            if (BVApp.CurrentStore == null)
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
            ThemeManager themes = BVApp.ThemeManager();

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
