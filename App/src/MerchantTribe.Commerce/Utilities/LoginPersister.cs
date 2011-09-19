using System;
using System.Text;
using System.IO;
using MerchantTribe.Web.Cryptography;

namespace MerchantTribe.Commerce.Utilities
{
    //public class LoginPersister : System.Web.IHttpModule
    //{

        //private void app_BeginRequest(object sender, System.EventArgs e)
        //{
        //    if (WebAppSettings.SiteRootsOnDifferentTLDs) {
        //        System.Web.HttpApplication app = (System.Web.HttpApplication)sender;
        //        object obj = app.Request.QueryString[WebAppSettings.SessionId];
        //        bool needRedirect = false;
        //        if (obj != null) {
        //            needRedirect = true;
        //            string sessionId = (string)obj;
        //            if (sessionId.Trim() != string.Empty) {
        //                TripleDesEncryption enc = new TripleDesEncryption();
        //                try {
        //                    SessionManager.SetCurrentUserId(enc.Decode(sessionId), WebAppSettings.RememberUsers);
        //                }
        //                catch (Exception ex) {
        //                    EventLog.LogEvent("LoginPersister", ex.ToString(), Metrics.EventLogSeverity.Error);
        //                }
        //            }
        //        }

        //        obj = app.Request.QueryString[WebAppSettings.CartId];
        //        if (obj != null) {
        //            needRedirect = true;
        //            string cartId = (string)obj;
        //            if (cartId.Trim() != string.Empty) {
        //                TripleDesEncryption enc = new TripleDesEncryption();
        //                try {
        //                    SessionManager.CurrentCartID = enc.Decode(cartId);
        //                }
        //                catch (Exception ex) {
        //                    EventLog.LogEvent("LoginPersister", ex.ToString(), Metrics.EventLogSeverity.Error);
        //                }
        //            }
        //        }

        //        if (needRedirect) {
        //            Uri uri = new Uri(UrlRewriter.GetRewrittenUrlFromRequest(app.Request));
        //            string url = uri.AbsolutePath;
        //            bool foundParams = false;
        //            foreach (string key in System.Web.HttpUtility.ParseQueryString(uri.Query)) {
        //                if (key != WebAppSettings.SessionId && key != WebAppSettings.CartId) {
        //                    if (!foundParams) {
        //                        url = url + "?";
        //                        foundParams = true;
        //                    }
        //                    url = url + key + "=" + System.Web.HttpUtility.UrlEncode(app.Request.QueryString[key]) + "&";
        //                }
        //            }
        //            if (url.EndsWith("&")) {
        //                url = url.TrimEnd('&');
        //            }
        //            app.Response.Redirect(url);
        //        }
        //    }
        //}

        //public void Dispose()
        //{

        //}

        //public void Init(System.Web.HttpApplication context)
        //{
        //    //AddHandler context.BeginRequest, AddressOf app_BeginRequest
        //    context.PostAcquireRequestState += app_BeginRequest;
        //}
	//}
}
