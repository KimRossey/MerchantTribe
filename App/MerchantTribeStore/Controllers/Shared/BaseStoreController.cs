using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Content;
using BVCommerce.Filters;

namespace BVCommerce.Controllers.Shared
{
    [StoreClosedFilter]
    public class BaseStoreController : Controller, IMultiStorePage
    {
        // Initialize Store Specific Request Data
        MerchantTribe.Commerce.RequestContext _BVRequestContext = new RequestContext();

        public MerchantTribeApplication MTApp { get; set; }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            MTApp = MerchantTribeApplication.InstantiateForDataBase(new RequestContext());

            // Check for non-www url and redirect if needed
            //RedirectBVCommerceCom(System.Web.HttpContext.Current);

            MTApp.CurrentRequestContext.RoutingContext = this.Request.RequestContext;

            // Determine store id        
            MTApp.CurrentStore = MerchantTribe.Commerce.Utilities.UrlHelper.ParseStoreFromUrl(System.Web.HttpContext.Current.Request.Url, MTApp.AccountServices);
            if (MTApp.CurrentStore == null)
            {
                Response.Redirect("~/storenotfound");
            }

            if (MTApp.CurrentStore.Status == MerchantTribe.Commerce.Accounts.StoreStatus.Deactivated)
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
                itemCount = SessionManager.GetCookieString(WebAppSettings.CookieNameCartItemCount(MTApp.CurrentStore.Id));
                subTotal = SessionManager.GetCookieString(WebAppSettings.CookieNameCartSubTotal(MTApp.CurrentStore.Id));
                if (itemCount.Trim().Length < 1) itemCount = "0";
                if (subTotal.Trim().Length < 1) subTotal = "$0.00";
            }
            ViewData["CurrentCartItemCount"] = itemCount;
            ViewData["CurrentCartSubTotal"] = subTotal;

            // style sheet
            ThemeManager themes = MTApp.ThemeManager();
            ViewBag.Css = themes.CurrentStyleSheet(System.Web.HttpContext.Current.Request.IsSecureConnection);

            // Add Google Tracker to Page
            if (MTApp.CurrentStore.Settings.Analytics.UseGoogleTracker)
            {
                ViewData["analyticstop"] = MerchantTribe.Commerce.Metrics.GoogleAnalytics.RenderLatestTracker(MTApp.CurrentStore.Settings.Analytics.GoogleTrackerId);
            }

            // Additional Meta Tags
            ViewData["AdditionalMetaTags"] = MerchantTribe.Web.HtmlSanitizer.MakeHtmlSafe(MTApp.CurrentStore.Settings.Analytics.AdditionalMetaTags);

            // JQuery
            ViewBag.JqueryInclude = Helpers.Html.JQueryIncludes(Url.Content("~/scripts"), this.Request.IsSecureConnection);

            // header and footer            
            string header = MerchantTribe.Commerce.Storage.DiskStorage.ReadCustomHeader(MTApp.CurrentStore.Id, MTApp.CurrentStore.Settings.ThemeId);
            string footer = MerchantTribe.Commerce.Storage.DiskStorage.ReadCustomFooter(MTApp.CurrentStore.Id, MTApp.CurrentStore.Settings.ThemeId);
            if ((header.Trim().Length < 1))
            {
                header = MerchantTribe.Commerce.Utilities.HtmlRendering.StandardHeader();
            }
            ViewData["siteheader"] = MerchantTribe.Commerce.Utilities.TagReplacer.ReplaceContentTags(header, MTApp, itemCount, Request.IsSecureConnection);
            if ((footer.Trim().Length < 1))
            {
                footer = MerchantTribe.Commerce.Utilities.HtmlRendering.StandardFooter(MTApp.CurrentStore);
            }
            footer = footer + MerchantTribe.Commerce.Utilities.HtmlRendering.PromoTag();
            ViewData["sitefooter"] = MerchantTribe.Commerce.Utilities.TagReplacer.ReplaceContentTags(footer, MTApp, itemCount, Request.IsSecureConnection);

            //log affiliate request
            if (!((string)Request.Params[WebAppSettings.AffiliateQueryStringName] == null))
            {
                string affid = string.Empty;
                try
                {
                    affid = Request.Params[WebAppSettings.AffiliateQueryStringName];
                    string referrerURL = Request.UrlReferrer.AbsoluteUri;
                    if (referrerURL == null) referrerURL = string.Empty;
                    MTApp.ContactServices.RecordAffiliateReferral(affid, referrerURL);

                }
                catch (System.Exception ex)
                {
                    EventLog.LogEvent("BaseStorePage - Page_Init", "Error loading affiliate " + ex.Message, MerchantTribe.Commerce.Metrics.EventLogSeverity.Warning);
                }
            }

            //If this is a private store, force login before showing anything.
            if (MTApp.CurrentStore.Settings.IsPrivateStore == true)
            {
                if (SessionManager.IsUserAuthenticated(this.MTApp) == false)
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
            ViewBag.IsAdmin = IsCurrentUserAdmin(this.MTApp, this.Request.RequestContext.HttpContext);
            ViewBag.RootUrlSecure = MTApp.CurrentStore.RootUrlSecure();
            ViewBag.RootUrl = MTApp.CurrentStore.RootUrl();
            ViewBag.StoreClosed = MTApp.CurrentStore.Settings.StoreClosed;


            ViewBag.MetaKeywords = MTApp.CurrentStore.Settings.MetaKeywords;            
            ViewBag.MetaDescription = MTApp.CurrentStore.Settings.MetaDescription;                                            

            // Integrations
            IntegrationLoader.AddIntegrations(this.MTApp.CurrentRequestContext.IntegrationEvents, this.MTApp.CurrentStore);
        }

        public bool IsCurrentUserAdmin(MerchantTribeApplication app, HttpContextBase httpContext)
        {

            Guid? tokenId = MerchantTribe.Web.Cookies.GetCookieGuid(
                                WebAppSettings.CookieNameAuthenticationTokenAdmin(),
                                httpContext, new EventLog());

            // no token, return
            if (!tokenId.HasValue) return false;

            if (app.AccountServices.IsTokenValidForStore(app.CurrentStore.Id, tokenId.Value))
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
