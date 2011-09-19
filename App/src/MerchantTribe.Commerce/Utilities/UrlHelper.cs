using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Commerce.Accounts;

namespace MerchantTribe.Commerce.Utilities
{
    public class UrlHelper
    {
        public static void RedirectToMainStoreUrl(long storeId, System.Uri requestedUrl, AccountService accountService)
        {
            Accounts.Store store = accountService.Stores.FindById(storeId);
            if (store == null) return;

            string destination = store.RootUrl();
            if (requestedUrl.ToString().ToLowerInvariant().StartsWith("https://"))
            {
                destination = store.RootUrlSecure();
            }

            
            string pathAndQuery = requestedUrl.PathAndQuery;
            // Trim starting slash because root URL already has this
            pathAndQuery = pathAndQuery.TrimStart('/');

            destination = System.IO.Path.Combine(destination, pathAndQuery);

            // 301 redirect to main url
            if (System.Web.HttpContext.Current != null)
            {
                System.Web.HttpContext.Current.Response.RedirectPermanent(destination);
            }
        }

        public static long GetStoreIdForCustomUrl(System.Uri url, AccountService accountService)
        {
            string host = url.DnsSafeHost.ToLowerInvariant();
            long result = accountService.Stores.FindStoreIdByCustomUrl(host);

            if (result < 1)
            {
                // Check other custom domains
                Accounts.StoreDomainRepository repo = new Accounts.StoreDomainRepository(RequestContext.GetCurrentRequestContext());
                Accounts.StoreDomain possible = repo.FindForAnyStoreByDomain(host);
                if (possible != null)
                {
                    if (possible.StoreId > 0)
                    {
                        RedirectToMainStoreUrl(possible.StoreId, url, accountService);
                    }
                }
            }

            return result;
        }

        public static string ParseStoreName(System.Uri url)
        {
            string result = string.Empty;

            string working = url.ToString().ToLowerInvariant();
            if (working.StartsWith("http://"))
            {
                working = working.Substring(7, working.Length - 7);
            }
            else if (working.StartsWith("https://"))
            {
                working = working.Substring(8, working.Length - 8);
            }

#if DEBUG
            if (working.StartsWith("localhost:")) return "sample";
#endif


            string[] parts = working.Split('.');
            if (parts != null)
            {
                // Need at least 2 parts to be a valid 
                // i.e. www.localhost
                // i.e. store.ecommrc.com
                if (parts.Length > 1)
                {
                    result = parts[0];
                }
            }

            return result;
        }

        public static long ParseStoreId(System.Uri url, AccountService accountService)
        {
            long result = -1;

            // Individual Mode
            if (WebAppSettings.IsIndividualMode)
            {
                Accounts.Store temp = accountService.FindOrCreateIndividualStore();
                if (temp != null) return temp.Id;
            }

            string host = url.DnsSafeHost.ToLowerInvariant();
            string mainDomain = WebAppSettings.ApplicationBaseUrl;
            // Trim off http://www
            if (mainDomain.Length > 11)
            {
                mainDomain = mainDomain.Substring(10, mainDomain.Length - 10);
                mainDomain = mainDomain.TrimEnd('/');
            }

            
            if (host.EndsWith(mainDomain))
            {
                // see if we're matching site domain first
                string storeName = ParseStoreName(url);
                Accounts.Store current = accountService.Stores.FindByStoreName(storeName);
                if (current != null)
                {
                    if (current.Id > 0)
                    {
                        result = current.Id;
                    }
                }                
            }
            else
            {
                // not on main domain, try custom urls
                result = GetStoreIdForCustomUrl(url, accountService);
            }

            return result;                           
        }


        // Primary Method to Detect Store from Uri
        public static Accounts.Store ParseStoreFromUrl(System.Uri url, AccountService accountService)
        {
            // Individual Mode
            if (WebAppSettings.IsIndividualMode)
            {
                return accountService.FindOrCreateIndividualStore();
            }

            // Debug Helper
            if (url.ToString().StartsWith("http://localhost:8888"))
            {
                url = new System.Uri(url.ToString().Replace("http://localhost:8888", "http://www.samplelocalhost6.com"));
            }
            if (url.ToString().StartsWith("https://localhost:8888"))
            {
                url = new System.Uri(url.ToString().Replace("https://localhost:8888", "https://www.samplelocalhost6.com"));
            }

            // Multi Mode
            Accounts.Store result = null;

            long storeid = ParseStoreId(url, accountService);
            if (storeid > 0)
            {
                result = accountService.Stores.FindById(storeid);
            }
            return result;
        }
    }
}
