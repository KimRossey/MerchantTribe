using System;
using System.Net;
using System.IO;
using System.Text;

namespace MerchantTribe.Commerce.Utilities
{

	public class WebForms
	{

		private const int DefaultTimeout = 100000;

		public static string SendRequestByPost(string serviceUrl, string postData)
		{
			return SendRequestByPost(serviceUrl, postData, null, DefaultTimeout);
		}

		public static string SendRequestByPost(string serviceUrl, string postData, System.Net.WebProxy proxy, int timeout)
		{
			WebResponse objResp;
			WebRequest objReq;
			string strResp = string.Empty;
			byte[] byteReq;

			try {
				byteReq = Encoding.UTF8.GetBytes(postData);
				objReq = WebRequest.Create(serviceUrl);
				objReq.Method = "POST";
				objReq.ContentLength = byteReq.Length;
				objReq.ContentType = "application/x-www-form-urlencoded";
				objReq.Timeout = timeout;
				if (proxy != null) {
					objReq.Proxy = proxy;
				}
				Stream OutStream = objReq.GetRequestStream();
				OutStream.Write(byteReq, 0, byteReq.Length);
				OutStream.Close();
				objResp = objReq.GetResponse();
				StreamReader sr = new StreamReader(objResp.GetResponseStream(), Encoding.UTF8, true);
				strResp += sr.ReadToEnd();
				sr.Close();
			}
			catch (Exception ex) {
				throw new ArgumentException("Error SendRequest: " + ex.Message + " " + ex.Source);
			}

			return strResp;
		}

		public static void MakePageNonCacheable(System.Web.UI.Page currentPage)
		{
			currentPage.Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
			currentPage.Response.Cache.SetNoStore();
			currentPage.Response.Cache.SetExpires(DateTime.Now.AddDays(-5000));
			currentPage.Response.Cache.SetMaxAge(new TimeSpan(0));
			currentPage.Response.Cache.AppendCacheExtension("must-revalidate, proxy-revalidate");

            //System.Web.UI.HtmlControls.HtmlMeta Expires = new System.Web.UI.HtmlControls.HtmlMeta();
            //Expires.Name = "Expires";
            //Expires.Content = "0";
            //currentPage.Header.Controls.Add(Expires);

            //System.Web.UI.HtmlControls.HtmlMeta CacheControl = new System.Web.UI.HtmlControls.HtmlMeta();
            //CacheControl.Name = "Cache-Control";
            //CacheControl.Content = "no-store";
            //currentPage.Header.Controls.Add(CacheControl);

            //System.Web.UI.HtmlControls.HtmlMeta Pragma = new System.Web.UI.HtmlControls.HtmlMeta();
            //Pragma.Name = "Pragma";
            //Pragma.Content = "no-cache";
            //currentPage.Header.Controls.Add(Pragma);
		}

	}
}
