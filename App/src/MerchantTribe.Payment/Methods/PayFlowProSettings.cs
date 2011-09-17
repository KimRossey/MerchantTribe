using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Payment.Methods
{
    public class PayFlowProSettings : MethodSettings
    {
         
        public string MerchantLogin
        {
            get { return GetSettingOrEmpty("MerchantLogin"); }
            set { this.AddOrUpdate("MerchantLogin", value); }
        }
        public string MerchantPartner
        {
            get { return GetSettingOrEmpty("MerchantPartner"); }
            set { this.AddOrUpdate("MerchantPartner", value); }
        }
        public string MerchantUser
        {
            get { return GetSettingOrEmpty("MerchantUser"); }
            set { this.AddOrUpdate("MerchantUser", value); }
        }
        public string MerchantPassword
        {
            get { return GetSettingOrEmpty("MerchantPassword"); }
            set { this.AddOrUpdate("MerchantPassword", value); }
        }
        public string CurrencyCode
        {
            get { return GetSettingOrEmpty("CurrencyCode"); }
            set { this.AddOrUpdate("CurrencyCode", value); }
        }
        public bool TestMode
        {
            get { return GetBoolSetting("TestMode"); }
            set { SetBoolSetting("TestMode", value); }
        }
        public bool DeveloperMode
        {
            get { return GetBoolSetting("DeveloperMode"); }
            set { SetBoolSetting("DeveloperMode", value); }
        }
        
    }
}
