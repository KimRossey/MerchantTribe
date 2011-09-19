using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Accounts
{
    public class StoreDomainSnapshot
    {
        public long Id { get; set; }
        public string StoreName { get; set; }
        public string CustomUrl { get; set; }

        public StoreDomainSnapshot()
        {
            Id = 0;
            StoreName = string.Empty;
            CustomUrl = string.Empty;
        }

        public bool HasCustomUrl
        {
            get
            {
                if (this.CustomUrl.Trim().ToLowerInvariant().Length > 0)
                {
                    return true;
                }
                return false;
            }
        }
        public string RootUrl()
        {
            // Individual Mode
            if (WebAppSettings.IsIndividualMode)
            {
                return WebAppSettings.BaseApplicationUrl;
            }

            // Multi Mode
            string result = string.Empty;

            if (this.HasCustomUrl)
            {
                result = "http://" + this.CustomUrl + "/";
            }
            else
            {
                result = StoreName;
                if (!StoreName.StartsWith("http"))
                {
                    result = WebAppSettings.BaseApplicationUrl.Replace("www", StoreName);
                }
            }
            return result;
        }
        public string RootUrlSecure()
        {
            string result = RootUrl();
            result = result.Replace("http://", "https://");
            return result;
        }
    }
}
