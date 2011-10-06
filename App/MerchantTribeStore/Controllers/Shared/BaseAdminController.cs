using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Accounts;

namespace MerchantTribeStore.Controllers.Shared
{
    public class BaseAdminController : Controller
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
        private AccountService _AccountService = null;
        public AccountService AccountServices
        {
            get
            {
                if (_AccountService == null)
                {
                    _AccountService = AccountService.InstantiateForDatabase(_CurrentRequestContext);
                }
                return _AccountService;
            }
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            // Store routing context for URL Rewriting
            CurrentRequestContext.RoutingContext = this.Request.RequestContext;

            // Determine store id        
            CurrentStore = MerchantTribe.Commerce.Utilities.UrlHelper.ParseStoreFromUrl(System.Web.HttpContext.Current.Request.Url, AccountServices);
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

            IntegrationLoader.AddIntegrations(this._CurrentRequestContext.IntegrationEvents, this.CurrentStore);

            ValidateAdminLogin();


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

            //ViewData["headerhtml"] = Helpers.Html.AdminHeader(CurrentStore, AdminTabType.None);
        }

        public void ValidateAdminLogin()
        {
            bool validLogin = false;

            Guid? tokenId = MerchantTribe.Web.Cookies.GetCookieGuid(WebAppSettings.CookieNameAuthenticationTokenAdmin(),
                    this.HttpContext,
                    new EventLog());

            if (tokenId.HasValue)
            {
                if (this.AccountServices.IsTokenValidForStore(CurrentStore.Id, tokenId.Value))
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
