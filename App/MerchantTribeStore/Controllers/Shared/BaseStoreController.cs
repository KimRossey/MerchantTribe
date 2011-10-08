﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Content;
using MerchantTribeStore.Filters;

namespace MerchantTribeStore.Controllers.Shared
{
    [StoreClosedFilter]
    public class BaseStoreController : BaseAppController
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            
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
                    EventLog.LogEvent("BaseStorePage - Page_Init", "Error loading affiliate " + ex.Message, MerchantTribe.Web.Logging.EventLogSeverity.Warning);
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
            
            ViewBag.MetaKeywords = MTApp.CurrentStore.Settings.MetaKeywords;            
            ViewBag.MetaDescription = MTApp.CurrentStore.Settings.MetaDescription;                                            
        }
    }
}
