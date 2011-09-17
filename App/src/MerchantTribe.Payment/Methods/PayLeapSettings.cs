using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Payment.Methods
{
    public class PayLeapSettings : MethodSettings
    {
        public string Username
        {
            get { return GetSettingOrEmpty("Username"); }
            set { this.AddOrUpdate("Username", value); }
        }
        public string Password
        {
            get { return GetSettingOrEmpty("Password"); }
            set { this.AddOrUpdate("Password", value); }
        }
        public bool TrainingMode
        {
            get { return GetBoolSetting("TrainingMode"); }
            set { SetBoolSetting("TrainingMode", value); }
        }
        public bool DeveloperMode
        {
            get { return GetBoolSetting("DeveloperMode"); }
            set { SetBoolSetting("DeveloperMode", value); }
        }

        public bool EnableDebugTracing
        {
            get { return GetBoolSetting("EnableDebugTracing"); }
            set { SetBoolSetting("EnableDebugTracing", value); }
        }
    }
}
