
using MerchantTribe.Commerce;
using System.Data;
using System.Collections.ObjectModel;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Accounts;

namespace MerchantTribeStore
{
    public class BaseStoreJsonPage : System.Web.UI.Page, IStorePage, MerchantTribe.Commerce.IMultiStorePage
    {

        private bool _useTabIndexes = false;
        private bool _AvailableWhenInactive = false;
        

        public bool AvailableWhenInactive
        {
            get { return _AvailableWhenInactive; }
            set { _AvailableWhenInactive = value; }
        }

        public bool UseTabIndexes
        {
            get { return _useTabIndexes; }
            set { _useTabIndexes = value; }
        }
        public virtual bool DisplaysActiveCategoryTab
        {
            get { return false; }
        }
        public virtual bool IsClosedPage
        {
            get { return false; }
        }

        public System.Web.Mvc.ViewDataDictionary ViewData
        {
            get { return ((IStorePage)this.Master).ViewData; }
            set { ((IStorePage)this.Master).ViewData = value; }
        }

        public MerchantTribeApplication MTApp {get;set;}
              
        public void AddBodyClass(string css)
        {
            ((IStorePage)this.Master).AddBodyClass(css);
        }

        // Redirect to the "www" version of the URL if needed
        private void RedirectBVCommerceCom(System.Web.HttpContext context)
        {
            // Bail out if we're in individual mode
            if (WebAppSettings.IsIndividualMode) return;

            if (context != null)
            {
                System.Uri url = context.Request.Url;
                string host = url.DnsSafeHost.ToLowerInvariant();
                if ("bvcommerce.com" == host)
                {
                    Response.RedirectPermanent("http://www.bvcommerce.com");
                }
            }
        }

        protected override void OnPreInit(System.EventArgs e)
        {
            base.OnPreInit(e);
            MTApp = MerchantTribeApplication.InstantiateForDataBase(new RequestContext());

            // Store routing context for URL Rewriting
            MTApp.CurrentRequestContext.RoutingContext = this.Request.RequestContext;

            // Check for non-www url and redirect if needed            
            RedirectBVCommerceCom(System.Web.HttpContext.Current);

            // Determine store id        
            MTApp.CurrentStore = MerchantTribe.Commerce.Utilities.UrlHelper.ParseStoreFromUrl(System.Web.HttpContext.Current.Request.Url, MTApp.AccountServices);
            if (MTApp.CurrentStore == null)
            {
                Response.Redirect("~/storenotfound");
            }

            if (MTApp.CurrentStore.Status == MerchantTribe.Commerce.Accounts.StoreStatus.Deactivated)
            {
                if ((AvailableWhenInactive == false))
                {
                    Response.Redirect("~/storenotavailable");
                }
            }

            // Culture Settings
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(MTApp.CurrentStore.Settings.CultureCode);
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(MTApp.CurrentStore.Settings.CultureCode);

            //If this is a private store, force login before showing anything.
            if (MTApp.CurrentStore.Settings.IsPrivateStore == true)
            {
                if (SessionManager.IsUserAuthenticated(this.MTApp) == false)
                {
                    string nameOfPage = Request.AppRelativeCurrentExecutionFilePath;
                    // Check to make sure we're not going to end up in an endless loop of redirects
                    if ((!nameOfPage.ToLower().StartsWith("~/login.aspx")) && (!nameOfPage.ToLower().StartsWith("~/account/forgotpassword")) && (!nameOfPage.ToLower().StartsWith("~/contactus")))
                    {
                        Response.Redirect("~/Login.aspx?ReturnUrl=" + System.Web.HttpUtility.UrlEncode(this.Request.RawUrl));
                    }
                }
            }

            IntegrationLoader.AddIntegrations(this.MTApp.CurrentRequestContext.IntegrationEvents, this.MTApp.CurrentStore);
        }

        public virtual bool RequiresSSL
        {
            get { return false; }
        }

        protected virtual void StoreClosedCheck()
        {
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            StoreClosedCheck();

            if (RequiresSSL)
            {
                if (WebAppSettings.UseSsl)
                {
                    if (!Request.IsSecureConnection)
                    {
                        MerchantTribe.Commerce.Utilities.SSL.SSLRedirect(this, this.MTApp.CurrentStore, MerchantTribe.Commerce.Utilities.SSL.SSLRedirectTo.SSL);
                    }
                }
            }
            else
            {
                if (WebAppSettings.UseSsl)
                {
                    if (Request.IsSecureConnection)
                    {
                        MerchantTribe.Commerce.Utilities.SSL.SSLRedirect(this, this.MTApp.CurrentStore, MerchantTribe.Commerce.Utilities.SSL.SSLRedirectTo.NonSSL);
                    }
                }
            }

        }

    }

}