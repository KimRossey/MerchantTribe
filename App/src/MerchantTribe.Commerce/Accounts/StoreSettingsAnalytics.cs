using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Accounts
{
    public class StoreSettingsAnalytics
    {
        private StoreSettings parent = null;

        public StoreSettingsAnalytics(StoreSettings s)
        {
            parent = s;
        }

        public bool DisableMerchantTribeAnalytics
        {
            get { 
                if (!WebAppSettings.IsCommercialVersion) return false;
                return parent.GetPropBoolWithDefault("DisableMerchantTribeAnalytics", false);
            }
            set { parent.SetProp("DisableMerchantTribeAnalytics", value); }
        }

        public bool UseGoogleTracker
        {
            get { return parent.GetPropBool("UseGoogleTracker"); }
            set { parent.SetProp("UseGoogleTracker", value); }
        }
        public bool UseGoogleAdWords
        {
            get { return parent.GetPropBool("UseGoogleAdWords"); }
            set { parent.SetProp("UseGoogleAdWords", value); }
        }
        public bool UseGoogleEcommerce
        {
            get { return parent.GetPropBool("UseGoogleEcommerce"); }
            set { parent.SetProp("UseGoogleEcommerce", value); }
        }
        public bool UseYahooTracker
        {
            get { return parent.GetPropBool("UseYahooTracker"); }
            set { parent.SetProp("UseYahooTracker", value); }
        }
        public string GoogleTrackerId
        {
            get { return parent.GetProp("GoogleTrackerId"); }
            set { parent.SetProp("GoogleTrackerId", value); }
        }
        public string GoogleAdWordsId
        {
            get { return parent.GetProp("GoogleAdWordsId"); }
            set { parent.SetProp("GoogleAdWordsId", value); }
        }
        public string GoogleAdWordsLabel
        {
            get { return parent.GetProp("GoogleAdWordsLabel"); }
            set { parent.SetProp("GoogleAdWordsLabel", value); }
        }
        public string GoogleAdWordsBgColor
        {
            get { return parent.GetProp("GoogleAdWordsBgColor"); }
            set { parent.SetProp("GoogleAdWordsBgColor", value); }
        }
        public string GoogleEcommerceStoreName
        {
            get { return parent.GetProp("GoogleEcommerceStoreName"); }
            set { parent.SetProp("GoogleEcommerceStoreName", value); }
        }
        public string GoogleEcommerceCategory
        {
            get { return parent.GetProp("GoogleEcommerceCategory"); }
            set { parent.SetProp("GoogleEcommerceCategory", value); }
        }
        public string YahooAccountId
        {
            get { return parent.GetProp("YahooAccountId"); }
            set { parent.SetProp("YahooAccountId", value); }
        }
        public string GoogleVerificationMetaTag
        {
            get { return parent.GetProp("GoogleVerificationMetaTag"); }
            set { parent.SetProp("GoogleVerificationMetaTag", value); }
        }
        public string GoogleVerificationMetaTagName
        {
            get
            {
                string prop = parent.GetProp("GoogleVerificationMetaTagName");
                if (prop == string.Empty)
                { prop = "verify-v1"; }
                return prop;
            }
            set { parent.SetProp("GoogleVerificationMetaTagName", value); }
        }
        public string AdditionalMetaTags
        {
            get
            {
                string prop = parent.GetProp("AdditionalMetaTags");
                return prop;
            }
            set { parent.SetProp("AdditionalMetaTags", value); }
        }
    }
}
