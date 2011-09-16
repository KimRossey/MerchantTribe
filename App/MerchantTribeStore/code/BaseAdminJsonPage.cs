using System;
using BVSoftware.Commerce;
using BVSoftware.Commerce.Accounts;
using BVSoftware.Commerce.Catalog;
using BVSoftware.Commerce.Contacts;

namespace BVCommerce
{
    public class BaseAdminJsonPage : System.Web.UI.Page, BVSoftware.Commerce.Controls.IBaseAdminPage, IMultiStorePage
    {
        private Guid? _AuthTokenGuid = null;
        public Guid? AuthTokenGuid
        {
            get { return _AuthTokenGuid; }
            set { _AuthTokenGuid = value; }
        }

        public BVApplication BVApp { get; set; }
                              
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            BVApp = BVApplication.InstantiateForDataBase(new RequestContext());

            // Store routing context for URL Rewriting
            BVApp.CurrentRequestContext.RoutingContext = this.Request.RequestContext;

            // Determine store id        
            BVApp.CurrentStore = BVSoftware.Commerce.Utilities.UrlHelper.ParseStoreFromUrl(System.Web.HttpContext.Current.Request.Url, BVApp.AccountServices);
            if (BVApp.CurrentStore == null)
            {
                Response.Redirect("~/storenotfound");
            }

            if (BVApp.CurrentStore.Status == BVSoftware.Commerce.Accounts.StoreStatus.Deactivated)
            {
                Response.Redirect("~/storenotavailable");
            }

            // Culture Settings
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(BVApp.CurrentStore.Settings.CultureCode);
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(BVApp.CurrentStore.Settings.CultureCode);

            IntegrationLoader.AddIntegrations(this.BVApp.CurrentRequestContext.IntegrationEvents, this.BVApp.CurrentStore);

            ValidateAdminLogin();
        }

        public void ValidateAdminLogin()
        {
            bool validLogin = false;

            Guid? tokenId = MerchantTribe.Web.Cookies.GetCookieGuid(WebAppSettings.CookieNameAuthenticationTokenAdmin(),
                    this.Page.Request.RequestContext.HttpContext,
                    new EventLog());

            if (tokenId.HasValue)
            {
                if (this.BVApp.AccountServices.IsTokenValidForStore(BVApp.CurrentStore.Id, tokenId.Value))
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