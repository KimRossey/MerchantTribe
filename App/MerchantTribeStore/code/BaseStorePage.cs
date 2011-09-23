using System;
using System.Web;
using MerchantTribe.Commerce;
using System.Data;
using System.Collections.ObjectModel;
using MerchantTribe.Commerce.Accounts;
using MerchantTribe.Commerce.Content;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Contacts;

namespace MerchantTribeStore
{
    public class BaseStorePage : System.Web.UI.Page, IStorePage, MerchantTribe.Commerce.IMultiStorePage
    {

        private bool _useTabIndexes = false;
        private bool _AvailableWhenInactive = false;

        public MerchantTribeApplication MTApp { get; set; }

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

        protected override void OnLoad(EventArgs e)
        {                        
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {

                this.MetaKeywords = MTApp.CurrentStore.Settings.MetaKeywords;
                this.MetaDescription = MTApp.CurrentStore.Settings.MetaDescription;
            }

            StoreClosedCheck();

            if (RequiresSSL)
            {
                if ((!Request.IsSecureConnection))
                {
                    MerchantTribe.Commerce.Utilities.SSL.SSLRedirect(this, this.MTApp.CurrentStore, MerchantTribe.Commerce.Utilities.SSL.SSLRedirectTo.SSL);
                }
            }
            else
            {
                if ((Request.IsSecureConnection))
                {
                    MerchantTribe.Commerce.Utilities.SSL.SSLRedirect(this, this.MTApp.CurrentStore, MerchantTribe.Commerce.Utilities.SSL.SSLRedirectTo.NonSSL);
                }
            }
        }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            MTApp = MerchantTribeApplication.InstantiateForDataBase(new RequestContext());

            // Check for non-www url and redirect if needed
            RedirectBVCommerceCom(System.Web.HttpContext.Current);

            // Store routing context for URL Rewriting
            MTApp.CurrentRequestContext.RoutingContext = this.Request.RequestContext;

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

            //Cart Count

            string itemCount = "0";
            string subTotal = "$0.00";

            if (SessionManager.CurrentUserHasCart())
            {
                itemCount = SessionManager.GetCookieString(WebAppSettings.CookieNameCartItemCount(MTApp.CurrentStore.Id));
                subTotal = SessionManager.GetCookieString(WebAppSettings.CookieNameCartSubTotal(MTApp.CurrentStore.Id));
                if (itemCount.Trim().Length < 1) itemCount = "0";
                if (subTotal.Trim().Length < 1) subTotal = "$0.00";
            }
            ViewData["CurrentCartItemCount"] = itemCount;
            ViewData["CurrentCartSubTotal"] = subTotal;

            // style sheet
            ThemeManager themes = MTApp.ThemeManager();
            ViewData["css"] = themes.CurrentStyleSheet(System.Web.HttpContext.Current.Request.IsSecureConnection);

            // Additional Meta Tags
            ViewData["AdditionalMetaTags"] = MerchantTribe.Web.HtmlSanitizer.MakeHtmlSafe(MTApp.CurrentStore.Settings.Analytics.AdditionalMetaTags);

            // Add Google Tracker to Page
            if (MTApp.CurrentStore.Settings.Analytics.UseGoogleTracker)
            {
                ViewData["analyticstop"] = MerchantTribe.Commerce.Metrics.GoogleAnalytics.RenderLatestTracker(MTApp.CurrentStore.Settings.Analytics.GoogleTrackerId);
            }

            // header and footer
            string header = MerchantTribe.Commerce.Storage.DiskStorage.ReadCustomHeader(MTApp.CurrentStore.Id, MTApp.CurrentStore.Settings.ThemeId);
            string footer = MerchantTribe.Commerce.Storage.DiskStorage.ReadCustomFooter(MTApp.CurrentStore.Id, MTApp.CurrentStore.Settings.ThemeId);

            if ((header.Trim().Length < 1))
            {
                header = MerchantTribe.Commerce.Utilities.HtmlRendering.StandardHeader();
            }
            ViewData["header"] = MerchantTribe.Commerce.Utilities.TagReplacer.ReplaceContentTags(header, MTApp, itemCount, Page.Request.IsSecureConnection);
            if ((footer.Trim().Length < 1))
            {
                footer = MerchantTribe.Commerce.Utilities.HtmlRendering.StandardFooter(MTApp.CurrentStore);
            }
            footer = footer + MerchantTribe.Commerce.Utilities.HtmlRendering.PromoTag();
            ViewData["footer"] = MerchantTribe.Commerce.Utilities.TagReplacer.ReplaceContentTags(footer, MTApp, itemCount, Page.Request.IsSecureConnection);


            //log affiliate request
            if (!((string)Request.Params[WebAppSettings.AffiliateQueryStringName] == null))
            {
                string affid = string.Empty;
                try
                {
                    affid = Request.Params[WebAppSettings.AffiliateQueryStringName];

                    string referrerURL = HttpContext.Current.Request.UrlReferrer.AbsoluteUri;
                    if (referrerURL == null) referrerURL = string.Empty;
                    MTApp.ContactServices.RecordAffiliateReferral(affid, referrerURL);
                }
                catch (System.Exception ex)
                {
                    EventLog.LogEvent("BaseStorePage - Page_Init", "Error loading affiliate " + ex.Message, MerchantTribe.Commerce.Metrics.EventLogSeverity.Warning);
                }
            }
            
            // Check for GuestPassword
            if (Request.QueryString[WebAppSettings.GuestPasswordQueryStringName] != null)
            {
                SessionManager.StoreClosedGuestPasswordForCurrentUser = Request.QueryString[WebAppSettings.GuestPasswordQueryStringName];
            }

            //If this is a private store, force login before showing anything.
            if (MTApp.CurrentStore.Settings.IsPrivateStore == true)
            {
                if (SessionManager.IsUserAuthenticated(this.MTApp) == false)
                {
                    string nameOfPage = Request.AppRelativeCurrentExecutionFilePath;
                    // Check to make sure we're not going to end up in an endless loop of redirects
                    if ((!nameOfPage.ToLower().StartsWith("~/signin"))
                        && (!nameOfPage.ToLower().StartsWith("~/account/forgotpassword"))
                        && (!nameOfPage.ToLower().StartsWith("~/contactus")))
                    {
                        Response.Redirect("~/signin?ReturnUrl=" + HttpUtility.UrlEncode(this.Request.RawUrl));
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
            if (MTApp.CurrentStore.Settings.StoreClosed == true)
            {
                bool hasPass = false;
                string guestPass = SessionManager.StoreClosedGuestPasswordForCurrentUser;
                if (guestPass.Trim().Length > 0)
                {
                    if (guestPass == MTApp.CurrentStore.Settings.StoreClosedGuestPassword)
                    {
                        hasPass = true;
                    }
                }
                if (MTApp.CurrentRequestContext.IsAdmin() == false && hasPass == false)
                {
                    Response.Redirect("~/storeclosed");
                }
            }
        }

        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
            //If Not Page.IsPostBack Then
            //If Me.Page.Title = String.Empty Then
            //Me.Page.Title = CurrentStore.FriendlyName
            //End If
            //End If
        }

    }
}