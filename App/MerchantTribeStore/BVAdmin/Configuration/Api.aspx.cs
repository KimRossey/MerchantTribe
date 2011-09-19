using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce.Membership;
using MerchantTribe.Commerce.Accounts;
using System.Text;

namespace BVCommerce.BVAdmin.Configuration
{
    public partial class Api : BaseAdminPage
    {
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "API Settings";
            this.CurrentTab = AdminTabType.Configuration;
            ValidateCurrentUserHasPermission(SystemPermissions.SettingsView);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {
                LoadApiKeys();
                this.litTimeLimit.Text = BVApp.CurrentStore.Settings.AllowApiToClearUntil.ToString("u");
            }
        }
      
        private void LoadApiKeys()
        {
            List<ApiKey> keys = BVApp.AccountServices.ApiKeys.FindByStoreId(BVApp.CurrentStore.Id);
            
            StringBuilder sb = new StringBuilder();
            sb.Append("<ul class=\"apikeys\">");

            foreach (ApiKey key in keys)
            {
                sb.Append("<li><pre>");
                sb.Append(key.Key);
                sb.Append(" <a id=\"remove" + key.Id.ToString() + "\" href=\"#\" class=\"removeapikey\">Revoke</a>");
                sb.Append("</pre></li>");
            }

            sb.Append("</ul>");

            this.litApiKeys.Text = sb.ToString();
        }

        protected void lnkCreateApiKey_Click(object sender, EventArgs e)
        {
            this.MessageBox1.ClearMessage();

            ApiKey key = new ApiKey();
            key.StoreId = BVApp.CurrentStore.Id;
            string k = System.Guid.NewGuid().ToString();
            k = key.StoreId.ToString() + "-" + k;
            key.Key = k;

            if (BVApp.AccountServices.ApiKeys.Create(key))
            {
                this.MessageBox1.ShowOk("New API key created!");
            }
            else
            {
                this.MessageBox1.ShowWarning("Unable to add API key!");
            }

            this.LoadApiKeys();
        }

        protected void btnResetClearTime_Click(object sender, EventArgs e)
        {
            BVApp.CurrentStore.Settings.AllowApiToClearUntil = DateTime.UtcNow.AddHours(1);            
            BVApp.AccountServices.Stores.Update(BVApp.CurrentStore);
            this.litTimeLimit.Text = BVApp.CurrentStore.Settings.AllowApiToClearUntil.ToString("u");
            this.LoadApiKeys();
        }
    }
}