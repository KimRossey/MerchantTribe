
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Accounts;
using MerchantTribe.Commerce.Catalog;

namespace MerchantTribeStore
{

    partial class BVAdmin_Login : System.Web.UI.Page, IMultiStorePage
    {        
        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {

                CheckSSL();

                if (Request.QueryString["ReturnURL"] != null)
                {
                    this.RedirectToField.Value = Request.QueryString["ReturnURL"];
                }

                if (Request.QueryString["username"] != null)
                {
                    UsernameField.Text = Request.QueryString["username"];
                    this.PasswordField.Focus();
                }
                else
                {
                    this.UsernameField.Focus();
                }

                if (Request.QueryString["wizard"] != null)
                {
                    if (WebAppSettings.IsIndividualMode)
                    {
                        this.MessageBox1.ShowInformation("Default username: admin@merchanttribe.com<br />Default Password: password");
                    }
                    else
                    {
                        this.MessageBox1.ShowInformation("Enter the same email and password you just used to create your store.");
                    }                    
                }

                if (Request.QueryString["reset"] == "1")
                {
                    this.MessageBox1.ShowOk("Your password was reset. You can now login.");
                }

            }
        }

        private void CheckSSL()
        {
            if (!Request.IsSecureConnection)
            {
                MerchantTribe.Commerce.Utilities.SSL.SSLRedirect(this,
                    this.MTApp.CurrentStore,
                    MerchantTribe.Commerce.Utilities.SSL.SSLRedirectTo.SSL);
            }
        }

        protected void btnLogin_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            this.MessageBox1.ClearMessage();


            string email = this.UsernameField.Text.Trim();
            string password = this.PasswordField.Text.Trim();

            string err = string.Empty;

            if ((MTApp.AccountServices.LoginAdminUser(email, password, ref err, Page.Request.RequestContext.HttpContext)))
            {

                if (Request.QueryString["wizard"] != null)
                {
                    Response.Redirect("~/bvadmin/setupwizard/wizardtheme.aspx");
                }

                Response.Redirect("~/bvadmin");
            }
            else
            {
                this.MessageBox1.ShowError(err);
            }

        }

    }
}