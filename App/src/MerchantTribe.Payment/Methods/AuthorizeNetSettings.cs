using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Payment.Methods
{
    public class AuthorizeNetSettings: MethodSettings
    {       
        public string MerchantLoginId
        {
            get {return GetSettingOrEmpty("MerchantLoginId"); }
            set { this.AddOrUpdate("MerchantLoginId", value); }
        }
        public string TransactionKey
        {
            get {return GetSettingOrEmpty("TransactionKey"); }
            set { this.AddOrUpdate("TransactionKey", value); }
        }
        public bool TestMode
        {
            get {return GetBoolSetting("TestMode");}
            set {SetBoolSetting("TestMode",value);}
        }
        public bool DeveloperMode
        {
            get { return GetBoolSetting("DeveloperMode"); }
            set { SetBoolSetting("DeveloperMode", value); }
        }
        public bool SendEmailToCustomer
        {
            get { return GetBoolSetting("SendEmailToCustomer"); }
            set { SetBoolSetting("SendEmailToCustomer", value); }
        }        
    }
}
