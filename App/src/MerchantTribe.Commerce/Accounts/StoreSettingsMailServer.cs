using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Accounts
{
    public class StoreSettingsMailServer
    {
        private StoreSettings parent = null;

        public StoreSettingsMailServer(StoreSettings s)
        {
            this.parent = s;
        }
        
        public bool UseCustomMailServer
        {
            get { return parent.GetPropBool("UseCustomMailServer"); }
            set { parent.SetProp("UseCustomMailServer", value); }
        }
        public string HostAddress
        {
            get { return parent.GetProp("MailServer"); }
            set { parent.SetProp("MailServer", value); }
        }
        public bool UseAuthentication
        {
            get { return parent.GetPropBool("MailServerUseAuthentication"); }
            set { parent.SetProp("MailServerUseAuthentication", value); }
        }
        public string Username
        {
            get { return parent.GetProp("MailServerUsername"); }
            set { parent.SetProp("MailServerUsername", value); }
        }
        public string Password
        {
            get { return parent.GetProp("MailServerPassword"); }
            set { parent.SetProp("MailServerPassword", value); }
        }
        public string Port
        {
            get { return parent.GetProp("MailServerPort"); }
            set { parent.SetProp("MailServerPort", value); }
        }
        public bool UseSsl
        {
            get { return parent.GetPropBool("MailServerUseSsl"); }
            set { parent.SetProp("MailServerUseSsl", value); }
        }

        public string EmailForGeneral
        {
            get { return parent.GetProp("EmailForGeneral"); }
            set { parent.SetProp("EmailForGeneral", value); }
        }
        public string EmailForNewOrder
        {
            get { return parent.GetProp("EmailForNewOrder"); }
            set { parent.SetProp("EmailForNewOrder", value); }
        }
        public string FromEmail
        {
            get { return parent.GetProp("FromEmail"); }
            set { parent.SetProp("FromEmail", value); }
        }
    }
}
