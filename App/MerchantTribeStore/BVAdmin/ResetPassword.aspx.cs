using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BVSoftware.Commerce;
using BVSoftware.Commerce.Accounts;
using BVSoftware.Commerce.Catalog;

namespace BVCommerce.BVAdmin
{
    public partial class ResetPassword : System.Web.UI.Page, IMultiStorePage
    {
        public BVApplication BVApp { get; set; }
        
        protected override void OnPreInit(System.EventArgs e)
        {
            base.OnPreInit(e);
            BVApp = BVApplication.InstantiateForDataBase(new RequestContext());

            // Determine store id        
            BVApp.CurrentStore = BVSoftware.Commerce.Utilities.UrlHelper.ParseStoreFromUrl(System.Web.HttpContext.Current.Request.Url, BVApp.AccountServices);
            if (BVApp.CurrentStore == null)
            {
                Response.Redirect("~/storenotfound");
            }

            if (BVApp.CurrentStore.Status == BVSoftware.Commerce.Accounts.StoreStatus.Deactivated)
            {
                Response.Redirect("~/storenotavailable");
            }

            // Culture Settings
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(BVApp.CurrentStore.Settings.CultureCode);
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(BVApp.CurrentStore.Settings.CultureCode);

            IntegrationLoader.AddIntegrations(this.BVApp.CurrentRequestContext.IntegrationEvents, this.BVApp.CurrentStore);
        }
        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {
                CheckSSL();                
                this.UsernameField.Focus();                               
            }
        }
        private void CheckSSL()
        {
            if (!Request.IsSecureConnection)
            {
                BVSoftware.Commerce.Utilities.SSL.SSLRedirect(this,
                    this.BVApp.CurrentStore,
                    BVSoftware.Commerce.Utilities.SSL.SSLRedirectTo.SSL);
            }
        }

        protected void lnkReset_Click(object sender, EventArgs e)
        {
            if (BVApp.AccountServices.AdminUserResetRequest(this.UsernameField.Text.Trim(), BVApp.CurrentStore))
            {
                this.MessageBox1.ShowOk("Check your email for your reset password link.");
            }
            else
            {
                this.MessageBox1.ShowWarning("Please check your email address and try again.");
            }                      
        }
    }
}