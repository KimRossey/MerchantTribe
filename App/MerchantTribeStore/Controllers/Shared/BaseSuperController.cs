using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Accounts;

namespace BVCommerce.Controllers.Shared
{
    public class BaseSuperController : Controller
    {
        // Initialize Store Specific Request Data
        private Guid? _AuthTokenGuid = null;
        public Guid? AuthTokenGuid
        {
            get { return _AuthTokenGuid; }
            set { _AuthTokenGuid = value; }
        }

        MerchantTribe.Commerce.RequestContext _CurrentRequestContext = new RequestContext();
        public MerchantTribe.Commerce.RequestContext CurrentRequestContext
        {
            get { return _CurrentRequestContext; }
            set { _CurrentRequestContext = value; }
        }
        public MerchantTribe.Commerce.Accounts.Store CurrentStore
        {
            get { return _CurrentRequestContext.CurrentStore; }
            set { _CurrentRequestContext.CurrentStore = value; }
        }
        public BVApplication BVApp { get; set; }
        public UserAccount CurrentSuperUser
        {
            get
            {
                if (AuthTokenGuid.HasValue)
                {
                    return BVApp.AccountServices.FindAdminUserByAuthTokenId(AuthTokenGuid.Value);
                }
                return null;
            }
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
                        
            CurrentRequestContext.RoutingContext = this.Request.RequestContext;
            BVApp = new BVApplication(CurrentRequestContext);

            // Determine store id        
            CurrentStore = MerchantTribe.Commerce.Utilities.UrlHelper.ParseStoreFromUrl(System.Web.HttpContext.Current.Request.Url, BVApp.AccountServices);
            if (CurrentStore == null)
            {
                Response.Redirect("~/storenotfound");
            }

            if (CurrentStore.Status == MerchantTribe.Commerce.Accounts.StoreStatus.Deactivated)
            {
                Response.Redirect("~/storenotavailable");
            }

            // Culture Settings
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(CurrentStore.Settings.CultureCode);
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(CurrentStore.Settings.CultureCode);

            ValidateSuperLogin();

            // Wallpaper
            string wallpaper = "BrownStripes.jpg";
            string wall = SessionManager.GetCookieString("AdminWallpaper");
            if (wall != string.Empty)
            {
                wallpaper = wall;
            }
            ViewData["wallpaper"] = Url.Content("~/images/system/" + wallpaper);

            // Jquery
            ViewData["JQueryInclude"] = Helpers.Html.JQueryIncludes(Url.Content("~/scripts"), this.Request.IsSecureConnection);

            ViewData["headerhtml"] = Helpers.Html.SuperHeader(CurrentStore);
        }

        public void ValidateSuperLogin()
        {
            bool validLogin = false;

            Guid? tokenId = MerchantTribe.Web.Cookies.GetCookieGuid(WebAppSettings.CookieNameAuthenticationTokenAdmin(),
                    this.HttpContext,
                    new EventLog());

            if (tokenId.HasValue)
            {
                if (BVApp.AccountServices.IsTokenValidForSuperUser(tokenId.Value))
                {
                    validLogin = true;
                }
            }

            if (validLogin == false)
            {
                Response.Redirect("~/account/login");
            }

            _AuthTokenGuid = tokenId;
        }

    }
}
