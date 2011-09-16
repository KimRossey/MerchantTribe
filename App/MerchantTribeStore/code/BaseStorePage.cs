using System;
using System.Web;
using BVSoftware.Commerce;
using System.Data;
using System.Collections.ObjectModel;
using BVSoftware.Commerce.Accounts;
using BVSoftware.Commerce.Content;
using BVSoftware.Commerce.Catalog;
using BVSoftware.Commerce.Contacts;

namespace BVCommerce
{
    public class BaseStorePage : System.Web.UI.Page, IStorePage, BVSoftware.Commerce.IMultiStorePage
    {

        private bool _useTabIndexes = false;
        private bool _AvailableWhenInactive = false;

        public BVApplication BVApp { get; set; }

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

                this.MetaKeywords = BVApp.CurrentStore.Settings.MetaKeywords;
                this.MetaDescription = BVApp.CurrentStore.Settings.MetaDescription;
            }

            StoreClosedCheck();

            if (RequiresSSL)
            {
                if ((!Request.IsSecureConnection))
                {
                    BVSoftware.Commerce.Utilities.SSL.SSLRedirect(this, this.BVApp.CurrentStore, BVSoftware.Commerce.Utilities.SSL.SSLRedirectTo.SSL);
                }
            }
            else
            {
                if ((Request.IsSecureConnection))
                {
                    BVSoftware.Commerce.Utilities.SSL.SSLRedirect(this, this.BVApp.CurrentStore, BVSoftware.Commerce.Utilities.SSL.SSLRedirectTo.NonSSL);
                }
            }
        }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            BVApp = BVApplication.InstantiateForDataBase(new RequestContext());

            // Check for non-www url and redirect if needed
            RedirectBVCommerceCom(System.Web.HttpContext.Current);

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
                if ((AvailableWhenInactive == false))
                {
                    Response.Redirect("~/storenotavailable");
                }
            }

            // Culture Settings
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(BVApp.CurrentStore.Settings.CultureCode);
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(BVApp.CurrentStore.Settings.CultureCode);

            //Cart Count

            string itemCount = "0";
            string subTotal = "$0.00";

            if (SessionManager.CurrentUserHasCart())
            {
                itemCount = SessionManager.GetCookieString(WebAppSettings.CookieNameCartItemCount(BVApp.CurrentStore.Id));
                subTotal = SessionManager.GetCookieString(WebAppSettings.CookieNameCartSubTotal(BVApp.CurrentStore.Id));
                if (itemCount.Trim().Length < 1) itemCount = "0";
                if (subTotal.Trim().Length < 1) subTotal = "$0.00";
            }
            ViewData["CurrentCartItemCount"] = itemCount;
            ViewData["CurrentCartSubTotal"] = subTotal;

            // style sheet
            ThemeManager themes = BVApp.ThemeManager();
            ViewData["css"] = themes.CurrentStyleSheet(System.Web.HttpContext.Current.Request.IsSecureConnection);

            // Additional Meta Tags
            ViewData["AdditionalMetaTags"] = MerchantTribe.Web.HtmlSanitizer.MakeHtmlSafe(BVApp.CurrentStore.Settings.Analytics.AdditionalMetaTags);

            // Add Google Tracker to Page
            if (BVApp.CurrentStore.Settings.Analytics.UseGoogleTracker)
            {
                ViewData["analyticstop"] = BVSoftware.Commerce.Metrics.GoogleAnalytics.RenderLatestTracker(BVApp.CurrentStore.Settings.Analytics.GoogleTrackerId);
            }

            // header and footer
            string header = BVSoftware.Commerce.Storage.DiskStorage.ReadCustomHeader(BVApp.CurrentStore.Id, BVApp.CurrentStore.Settings.ThemeId);
            string footer = BVSoftware.Commerce.Storage.DiskStorage.ReadCustomFooter(BVApp.CurrentStore.Id, BVApp.CurrentStore.Settings.ThemeId);

            if ((header.Trim().Length < 1))
            {
                header = BVSoftware.Commerce.Utilities.HtmlRendering.StandardHeader();
            }
            ViewData["header"] = BVSoftware.Commerce.Utilities.TagReplacer.ReplaceContentTags(header, BVApp, itemCount, Page.Request.IsSecureConnection);
            if ((footer.Trim().Length < 1))
            {
                footer = BVSoftware.Commerce.Utilities.HtmlRendering.StandardFooter(BVApp.CurrentStore);
            }
            footer = footer + BVSoftware.Commerce.Utilities.HtmlRendering.PromoTag();
            ViewData["footer"] = BVSoftware.Commerce.Utilities.TagReplacer.ReplaceContentTags(footer, BVApp, itemCount, Page.Request.IsSecureConnection);


            //log affiliate request
            if (!((string)Request.Params[WebAppSettings.AffiliateQueryStringName] == null))
            {
                string affid = string.Empty;
                try
                {
                    affid = Request.Params[WebAppSettings.AffiliateQueryStringName];

                    string referrerURL = HttpContext.Current.Request.UrlReferrer.AbsoluteUri;
                    if (referrerURL == null) referrerURL = string.Empty;
                    BVApp.ContactServices.RecordAffiliateReferral(affid, referrerURL);
                }
                catch (System.Exception ex)
                {
                    EventLog.LogEvent("BaseStorePage - Page_Init", "Error loading affiliate " + ex.Message, BVSoftware.Commerce.Metrics.EventLogSeverity.Warning);
                }
            }
            
            // Check for GuestPassword
            if (Request.QueryString[WebAppSettings.GuestPasswordQueryStringName] != null)
            {
                SessionManager.StoreClosedGuestPasswordForCurrentUser = Request.QueryString[WebAppSettings.GuestPasswordQueryStringName];
            }

            //If this is a private store, force login before showing anything.
            if (BVApp.CurrentStore.Settings.IsPrivateStore == true)
            {
                if (SessionManager.IsUserAuthenticated(this.BVApp) == false)
                {
                    string nameOfPage = Request.AppRelativeCurrentExecutionFilePath;
                    // Check to make sure we're not going to end up in an endless loop of redirects
                    if ((!nameOfPage.ToLower().StartsWith("~/signin"))
                        && (!nameOfPage.ToLower().StartsWith("~/forgotpassword.aspx"))
                        && (!nameOfPage.ToLower().StartsWith("~/contactus.aspx")))
                    {
                        Response.Redirect("~/signin?ReturnUrl=" + HttpUtility.UrlEncode(this.Request.RawUrl));
                    }
                }
            }

            IntegrationLoader.AddIntegrations(this.BVApp.CurrentRequestContext.IntegrationEvents, this.BVApp.CurrentStore);
        }      

        public virtual bool RequiresSSL
        {
            get { return false; }
        }

        protected virtual void StoreClosedCheck()
        {
            if (BVApp.CurrentStore.Settings.StoreClosed == true)
            {
                bool hasPass = false;
                string guestPass = SessionManager.StoreClosedGuestPasswordForCurrentUser;
                if (guestPass.Trim().Length > 0)
                {
                    if (guestPass == BVApp.CurrentStore.Settings.StoreClosedGuestPassword)
                    {
                        hasPass = true;
                    }
                }
                if (BVApp.CurrentRequestContext.IsAdmin() == false && hasPass == false)
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