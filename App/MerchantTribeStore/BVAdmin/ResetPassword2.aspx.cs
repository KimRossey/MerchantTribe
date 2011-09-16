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
    public partial class ResetPassword2 : System.Web.UI.Page, IMultiStorePage
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
                this.PasswordField.Focus();
                this.UsernameField.Text = Request.QueryString["email"];
                this.ResetKeyField.Text = Request.QueryString["resetkey"];
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
            UserAccount u = BVApp.AccountServices.AdminUsers.FindByEmail(this.UsernameField.Text.Trim());
            if (u == null)
            {
                this.MessageBox1.ShowWarning("Check your email address and try again.");
                return;
            }
            if (u.Email == string.Empty)
            {
                this.MessageBox1.ShowWarning("Check your email address and try again.");
                return;
            }

            if (u.ResetPassword(this.ResetKeyField.Text.Trim(), this.PasswordField.Text.Trim()))
            {
                u.ResetKey = string.Empty; // Disable the key once it's been used.
                BVApp.AccountServices.AdminUsers.Update(u);
                Response.Redirect("~/account/login?reset=1");
            }
            else
            {
                this.MessageBox1.ShowWarning("Unable to reset password. Check your reset key and contact administrator for assistance.");
            }

        }
    }
}