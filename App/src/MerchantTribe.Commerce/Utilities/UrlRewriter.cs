using System;
using System.Data;
using System.Collections.ObjectModel;
using System.Text;
using System.IO;
using System.Web.Routing;

namespace MerchantTribe.Commerce.Utilities
{
	public class UrlRewriter
	{    
        public static bool IsProductSlugInUse(string slug, string bvin, MerchantTribeApplication app)
        {
            Catalog.Product p = app.CatalogServices.Products.FindBySlug(slug);
            if (p != null)
            {
                if (p.Bvin != string.Empty)
                {
                    if (p.Bvin != bvin)
                    {
                        return true;
                    }
                }
            }
            return false;            
        }        
        public static bool IsCategorySlugInUse(string slug, string bvin, RequestContext context)
        {
            Catalog.CategoryRepository repository = Catalog.CategoryRepository.InstantiateForDatabase(context);
            return IsCategorySlugInUse(slug, bvin, repository);
        }
        public static bool IsCategorySlugInUse(string slug, string bvin, Catalog.CategoryRepository repository)
        {            
            Catalog.Category c = repository.FindBySlug(slug);
            if (c != null)
            {
                if (c.Bvin != string.Empty)
                {
                    if (c.Bvin != bvin)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public static bool IsUrlInUse(string requestedUrl, string thisCustomUrlBvin, RequestContext context, MerchantTribeApplication app)
        {
            bool result = false;            
            string working = requestedUrl.ToLowerInvariant();

            // Check for Generic Page Use in a Flex Page
            if (IsCategorySlugInUse(working, thisCustomUrlBvin, context)) return true;

            // Check for Products
            if (IsProductSlugInUse(working, thisCustomUrlBvin, app)) return true;

            // Check Custom Urls
            Content.CustomUrl url = app.ContentServices.CustomUrls.FindByRequestedUrl(requestedUrl);
            if (url != null)
            {
                if (url.Bvin != string.Empty)
                {
                    if (url.Bvin != thisCustomUrlBvin)
                    {
                        return true;
                    }
                }
            }
            return result;
        }

		private static bool RedirectToCustomUrl(System.Web.HttpApplication app, Content.CustomUrl u, UrlRewriterParts parts)
		{
			if (u.RedirectToUrl.ToLower().StartsWith("http")) {
				if (parts.HasQuery) {
					app.Response.Redirect(JoinUrlAndQuery(u.RedirectToUrl, parts.Query));
					return true;
				}
				else {
					app.Response.Redirect(u.RedirectToUrl);
					return true;
				}
			}
			else {
				string outputPath = u.RedirectToUrl;
				outputPath = MakeRelativeCustomUrlSafeForApp(outputPath, app.Request.ApplicationPath);
				if (parts.HasQuery) {
					app.Context.RewritePath(JoinUrlAndQuery(outputPath, parts.Query), false);
					return true;
				}
				else {
					app.Context.RewritePath(outputPath, false);
					return true;
				}
			}			
		}			

		public static string MakeRelativeCustomUrlSafeForApp(string customUrl, string appPath)
		{
			string result = customUrl;

			string testAppPath = appPath.TrimEnd('/');

			if (appPath != "/") {
				if (testAppPath.Length > 0) {
					result = testAppPath + customUrl;
				}
			}

			return result;
		}

		public void Dispose()
		{

		}

        //public void Init(System.Web.HttpApplication context)
        //{
        //    context.BeginRequest += app_BeginRequest;
        //}

        //private void app_BeginRequest(object sender, System.EventArgs e)
        //{
        //    System.Web.HttpApplication app = (System.Web.HttpApplication)sender;
        //    RewritePath(ref app, app.Request.Url);
        //}

		public static string BuildUrlForCategory(Catalog.CategorySnapshot c, System.Web.Routing.RequestContext routingContext)
		{
            if (c.SourceType == Catalog.CategorySourceType.CustomLink)
            {
                if (c.CustomPageUrl != string.Empty) return c.CustomPageUrl;
            }
            if (c.RewriteUrl == string.Empty) return string.Empty;
            RouteCollection r = System.Web.Routing.RouteTable.Routes;
            return r.GetVirtualPath(routingContext, "bvroute", new RouteValueDictionary(new { slug = c.RewriteUrl })).VirtualPath.ToString();
		}
        public static string BuildUrlForCategory(Catalog.CategorySnapshot c, System.Web.Routing.RequestContext routingContext, string pageNumber)
        {
            if (c.SourceType == Catalog.CategorySourceType.CustomLink)
            {
                if (c.CustomPageUrl != string.Empty) return c.CustomPageUrl;
            }
            if (c.RewriteUrl == string.Empty) return string.Empty;
            RouteCollection r = System.Web.Routing.RouteTable.Routes;
            string result = r.GetVirtualPath(routingContext, "bvroute", new RouteValueDictionary(new { slug = c.RewriteUrl })).VirtualPath.ToString();
            result += "?page=" + pageNumber;
            return result;
        }
        public static string BuildUrlForCategory(Catalog.CategorySnapshot c, System.Web.Routing.RequestContext routingContext, string pageNumber, string filternode)
        {
            if (c.SourceType == Catalog.CategorySourceType.CustomLink)
            {
                if (c.CustomPageUrl != string.Empty) return c.CustomPageUrl;
            }
            if (c.RewriteUrl == string.Empty) return string.Empty;
            RouteCollection r = System.Web.Routing.RouteTable.Routes;
            string result = r.GetVirtualPath(routingContext, "bvroute", new RouteValueDictionary(new { slug = c.RewriteUrl })).VirtualPath.ToString();
            result += "?node=" + filternode;
            if (pageNumber != "1")
            {
                result += "&page=" + pageNumber;
            }
            return result;
        }	
		public static string BuildUrlForProduct(Catalog.Product p, System.Web.UI.Page page)
		{
            return BuildUrlForProduct(p, page, string.Empty);			
		}
		public static string BuildUrlForProduct(Catalog.Product p, System.Web.UI.Page page, string additionalParams)
		{            
            string result = string.Empty;

            if (p != null)
            {
                result = page.GetRouteUrl("bvroute", new { slug = p.UrlSlug });
            }
            else
            {
                result = page.GetRouteUrl("bvroute", new { slug = "" });
            }
			
            
            if (additionalParams != string.Empty)            
            {
                result = JoinUrlAndQuery(result, additionalParams);                
            }

            return result;
		}
        public static string BuildUrlForProduct(Catalog.Product p, System.Web.Routing.RequestContext routingContext, string additionalParams)
        {
            string result = string.Empty;

            RouteCollection r = System.Web.Routing.RouteTable.Routes;

            if (p != null)
            {
                result = r.GetVirtualPath(routingContext, "bvroute", new RouteValueDictionary(new { slug = p.UrlSlug })).VirtualPath.ToString();
            }
            else
            {
                result = r.GetVirtualPath(routingContext, "bvroute", new RouteValueDictionary(new { slug = "" })).VirtualPath.ToString();                
            }

            if (additionalParams != string.Empty)
            {
                result = JoinUrlAndQuery(result, additionalParams);
            }

            return result;
        }

		private static string CleanNameForUrl(string input)
		{
			string result = input;

			result = result.Replace(" ", "-");
			result = result.Replace("\"", "");
			result = result.Replace("&", "and");
			result = result.Replace("?", "");
			result = result.Replace("=", "");
			result = result.Replace("/", "");
			result = result.Replace("\\", "");
			result = result.Replace("%", "");
			result = result.Replace("#", "");
			result = result.Replace("*", "");
			result = result.Replace("!", "");
			result = result.Replace("$", "");
			result = result.Replace("+", "-plus-");
			result = result.Replace(",", "-");
			result = result.Replace("@", "-at-");
			result = result.Replace(":", "-");
			result = result.Replace(";", "-");
			result = result.Replace(">", "");
			result = result.Replace("<", "");
			result = result.Replace("{", "");
			result = result.Replace("}", "");
			result = result.Replace("~", "");
			result = result.Replace("|", "-");
			result = result.Replace("^", "");
			result = result.Replace("[", "");
			result = result.Replace("]", "");
			result = result.Replace("`", "");
			result = result.Replace("'", "");
			result = result.Replace("�", "");
			result = result.Replace("�", "");
			result = result.Replace("�", "");
			result = result.Replace(".", "");

			result = System.Web.HttpUtility.UrlDecode(result);

			return result;
		}
		private static string CleanSkuForUrl(string input)
		{
			string result = input;

			result = result.Replace(" ", "-spc-");
			result = result.Replace("\"", "-quot-");
			result = result.Replace("&", "-and-");
			result = result.Replace("?", "-ques-");
			result = result.Replace("=", "-eql-");
			result = result.Replace("/", "-fslsh-");
			result = result.Replace("\\", "-bslsh-");
			result = result.Replace("%", "-perc-");
			result = result.Replace("#", "-hash-");
			result = result.Replace("*", "-aste-");
			result = result.Replace("!", "-excl-");
			result = result.Replace("$", "-dolr-");
			result = result.Replace("+", "-plus-");
			result = result.Replace(",", "-comm-");
			result = result.Replace("@", "-at-");
			result = result.Replace(":", "-colo-");
			result = result.Replace(";", "-semc-");
			result = result.Replace(">", "-gt-");
			result = result.Replace("<", "-lt-");
			result = result.Replace("{", "-lcb-");
			result = result.Replace("}", "-rcb-");
			result = result.Replace("~", "-til-");
			result = result.Replace("|", "-pip-");
			result = result.Replace("^", "-crt-");
			result = result.Replace("[", "-lsqb-");
			result = result.Replace("]", "-rsqb");
			result = result.Replace("`", "-btck-");
			result = result.Replace("'", "-aps-");
			result = result.Replace("�", "-cpy-");
			result = result.Replace("�", "-tm-");
			result = result.Replace("�", "-rgtm-");
			result = result.Replace(".", "-prd-");

			result = System.Web.HttpUtility.UrlEncode(result);

			return result;
		}
		private static string GetUnEscapedSku(string input)
		{
			string result = input;

			result = result.Replace("-spc-", " ");
			result = result.Replace("-quot-", "\"");
			result = result.Replace("-and-", "&");
			result = result.Replace("-ques-", "?");
			result = result.Replace("-eql-", "=");
			result = result.Replace("-fslsh-", "/");
			result = result.Replace("-bslsh-", "\\");
			result = result.Replace("-perc-", "%");
			result = result.Replace("-hash-", "#");
			result = result.Replace("-aste-", "*");
			result = result.Replace("-excl-", "!");
			result = result.Replace("-dolr-", "$");
			result = result.Replace("-plus-", "+");
			result = result.Replace("-comm-", ",");
			result = result.Replace("-at-", "@");
			result = result.Replace("-colo-", ":");
			result = result.Replace("-semc-", ";");
			result = result.Replace("-gt-", ">");
			result = result.Replace("-lt-", "<");
			result = result.Replace("-lcb-", "{");
			result = result.Replace("-rcb-", "}");
			result = result.Replace("-til-", "~");
			result = result.Replace("-pip-", "|");
			result = result.Replace("-crt-", "^");
			result = result.Replace("-lsqb-", "[");
			result = result.Replace("-rsqb", "]");
			result = result.Replace("-btck-", "`");
			result = result.Replace("-aps-", "'");
			result = result.Replace("-cpy-", "�");
			result = result.Replace("-tm-", "�");
			result = result.Replace("-rgtm-", "�");
			result = result.Replace("-prd-", ".");

			return result;
		}

		public static string JoinUrlAndQuery(string url, string query)
		{
			if (url.Contains("?")) {
				return url + "&" + query;
			}
			else {
				return url + "?" + query;
			}
		}

		public static string GetRewrittenUrlFromRequest(System.Web.HttpRequest Request)
		{
			if (Request.RawUrl.StartsWith("/")) {
				return Request.Url.Scheme + "://" + Request.Url.Host + Request.RawUrl;
			}
			else {
				return Request.Url.Scheme + "://" + Request.Url.Host + "/" + Request.RawUrl;
			}
		}

		public static string SwitchUrlToSecure(string currentUrl)
		{
            if (currentUrl.StartsWith("http://"))
            {
                return currentUrl.Insert(4, "s");
            }
            return currentUrl;
            //string result = string.Empty;
            //result = System.Text.RegularExpressions.Regex.Replace(currentUrl, standardBaseUrl, secureBaseUrl);

            //if (string.Compare(result, currentUrl, true) == 0) {
            //    string standardShortenedUrl = System.Text.RegularExpressions.Regex.Replace(standardBaseUrl, "http://www.", "http://");
            //    result = System.Text.RegularExpressions.Regex.Replace(currentUrl, standardShortenedUrl, secureBaseUrl);
            //}
            //return result;
		}
		public static string SwitchUrlToStandard(string currentUrl)
		{
            if (currentUrl.StartsWith("https://"))
            {
                return currentUrl.Remove(4, 1);
            }
            return currentUrl;

            //string result = string.Empty;
            //result = System.Text.RegularExpressions.Regex.Replace(currentUrl, secureBaseUrl, standardBaseUrl);

            //if (string.Compare(result, currentUrl, true) == 0) {
            //    string secureShortenedUrl = System.Text.RegularExpressions.Regex.Replace(secureBaseUrl, "https://www.", "https://");
            //    result = System.Text.RegularExpressions.Regex.Replace(currentUrl, secureShortenedUrl, standardBaseUrl);
            //}

            //return result;
		}

		public static void RedirectToErrorPage(ErrorTypes errorType, System.Web.HttpResponse response)
		{
			if (errorType == ErrorTypes.Generic) {
				response.Redirect("~/Error");
			}
			else if (errorType == ErrorTypes.Product) {
				response.Redirect("~/Error?type=product");
			}
			else if (errorType == ErrorTypes.Category) {
				response.Redirect("~/Error?type=category");
			}
		}
	}
}

