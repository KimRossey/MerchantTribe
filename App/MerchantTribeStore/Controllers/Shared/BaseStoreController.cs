using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BVSoftware.Commerce;
using BVSoftware.Commerce.Content;
using BVCommerce.Filters;

namespace BVCommerce.Controllers.Shared
{
    [StoreClosedFilter]
    public class BaseStoreController : Controller, IMultiStorePage
    {
        // Initialize Store Specific Request Data
        BVSoftware.Commerce.RequestContext _BVRequestContext = new RequestContext();

        public BVApplication BVApp { get; set; }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            BVApp = BVApplication.InstantiateForDataBase(new RequestContext());

            // Check for non-www url and redirect if needed
            //RedirectBVCommerceCom(System.Web.HttpContext.Current);

            BVApp.CurrentRequestContext.RoutingContext = this.Request.RequestContext;

            // Determine store id        
            BVApp.CurrentStore = BVSoftware.Commerce.Utilities.UrlHelper.ParseStoreFromUrl(System.Web.HttpContext.Current.Request.Url, BVApp.AccountServices);
            if (BVApp.CurrentStore == null)
            {
                Response.Redirect("~/storenotfound");
            }

            if (BVApp.CurrentStore.Status == BVSoftware.Commerce.Accounts.StoreStatus.Deactivated)
            {
                //if ((AvailableWhenInactive == false))
                //{
                Response.Redirect("~/storenotavailable");
                //}
            }

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
            ViewBag.Css = themes.CurrentStyleSheet(System.Web.HttpContext.Current.Request.IsSecureConnection);

            // Add Google Tracker to Page
            if (BVApp.CurrentStore.Settings.Analytics.UseGoogleTracker)
            {
                ViewData["analyticstop"] = BVSoftware.Commerce.Metrics.GoogleAnalytics.RenderLatestTracker(BVApp.CurrentStore.Settings.Analytics.GoogleTrackerId);
            }

            // Additional Meta Tags
            ViewData["AdditionalMetaTags"] = MerchantTribe.Web.HtmlSanitizer.MakeHtmlSafe(BVApp.CurrentStore.Settings.Analytics.AdditionalMetaTags);

            // JQuery
            ViewBag.JqueryInclude = Helpers.Html.JQueryIncludes(Url.Content("~/scripts"), this.Request.IsSecureConnection);

            // header and footer            
            string header = BVSoftware.Commerce.Storage.DiskStorage.ReadCustomHeader(BVApp.CurrentStore.Id, BVApp.CurrentStore.Settings.ThemeId);
            string footer = BVSoftware.Commerce.Storage.DiskStorage.ReadCustomFooter(BVApp.CurrentStore.Id, BVApp.CurrentStore.Settings.ThemeId);
            if ((header.Trim().Length < 1))
            {
                header = BVSoftware.Commerce.Utilities.HtmlRendering.StandardHeader();
            }
            ViewData["siteheader"] = BVSoftware.Commerce.Utilities.TagReplacer.ReplaceContentTags(header, BVApp, itemCount, Request.IsSecureConnection);
            if ((footer.Trim().Length < 1))
            {
                footer = BVSoftware.Commerce.Utilities.HtmlRendering.StandardFooter(BVApp.CurrentStore);
            }
            footer = footer + BVSoftware.Commerce.Utilities.HtmlRendering.PromoTag();
            ViewData["sitefooter"] = BVSoftware.Commerce.Utilities.TagReplacer.ReplaceContentTags(footer, BVApp, itemCount, Request.IsSecureConnection);

            //log affiliate request
            if (!((string)Request.Params[WebAppSettings.AffiliateQueryStringName] == null))
            {
                string affid = string.Empty;
                try
                {
                    affid = Request.Params[WebAppSettings.AffiliateQueryStringName];
                    string referrerURL = Request.UrlReferrer.AbsoluteUri;
                    if (referrerURL == null) referrerURL = string.Empty;
                    BVApp.ContactServices.RecordAffiliateReferral(affid, referrerURL);

                }
                catch (System.Exception ex)
                {
                    EventLog.LogEvent("BaseStorePage - Page_Init", "Error loading affiliate " + ex.Message, BVSoftware.Commerce.Metrics.EventLogSeverity.Warning);
                }
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

            // Store data for admin panel
            ViewBag.IsAdmin = IsCurrentUserAdmin(this.BVApp, this.Request.RequestContext.HttpContext);
            ViewBag.RootUrlSecure = BVApp.CurrentStore.RootUrlSecure();
            ViewBag.RootUrl = BVApp.CurrentStore.RootUrl();
            ViewBag.StoreClosed = BVApp.CurrentStore.Settings.StoreClosed;


            ViewBag.MetaKeywords = BVApp.CurrentStore.Settings.MetaKeywords;            
            ViewBag.MetaDescription = BVApp.CurrentStore.Settings.MetaDescription;                                            

            // Integrations
            IntegrationLoader.AddIntegrations(this.BVApp.CurrentRequestContext.IntegrationEvents, this.BVApp.CurrentStore);
        }

        public bool IsCurrentUserAdmin(BVApplication bvapp, HttpContextBase httpContext)
        {

            Guid? tokenId = MerchantTribe.Web.Cookies.GetCookieGuid(
                                WebAppSettings.CookieNameAuthenticationTokenAdmin(),
                                httpContext, new EventLog());

            // no token, return
            if (!tokenId.HasValue) return false;

            if (bvapp.AccountServices.IsTokenValidForStore(bvapp.CurrentStore.Id, tokenId.Value))
            {
                return true;
            }

            return false;
        }


        protected void FlashInfo(string message)
        {
            FlashMessage(message, "flash-message-info");
        }
        protected void FlashSuccess(string message)
        {
            FlashMessage(message, "flash-message-success");
        }
        protected void FlashFailure(string message)
        {
            FlashMessage(message, "flash-message-failure");
        }
        protected void FlashWarning(string message)
        {
            FlashMessage(message, "flash-message-warning");
        }
        private void FlashMessage(string message, string typeClass)
        {
            string format = "<div class=\"{0}\"><p>{1}</p></div>";
            this.TempData["messages"] += string.Format(format, typeClass, message);
        }        
    }
}
