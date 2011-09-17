using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Payment.Methods
{
    public class PayPalPaymentsProSettings : MethodSettings
    {
        public string PayPalUserName
        {
            get { return GetSettingOrEmpty("PayPalUserName"); }
            set { this.AddOrUpdate("PayPalUserName", value); }
        }
        public string PayPalPassword
        {
            get { return GetSettingOrEmpty("PayPalPassword"); }
            set { this.AddOrUpdate("PayPalPassword", value); }
        }
        public string PayPalSignature
        {
            get { return GetSettingOrEmpty("PayPalSignature"); }
            set { this.AddOrUpdate("PayPalSignature", value); }
        }
        public string PayPalMode
        {
            get { return GetSettingOrEmpty("PayPalMode"); }
            set { this.AddOrUpdate("PayPalMode", value); }
        }
        
        //public bool TestMode
        //{
        //    get { return GetBoolSetting("TestMode"); }
        //    set { SetBoolSetting("TestMode", value); }
        //}
        public bool DebugMode
        {
            get { return GetBoolSetting("DebugMode"); }
            set { SetBoolSetting("DebugMode", value); }
        }
        //public bool SendEmailToCustomer
        //{
        //    get { return GetBoolSetting("SendEmailToCustomer"); }
        //    set { SetBoolSetting("SendEmailToCustomer", value); }
        //}
    }
}
