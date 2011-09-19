
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Accounts;
using MerchantTribe.Commerce.Catalog;

namespace BVCommerce
{

    partial class BVAdmin_Login : System.Web.UI.Page, IMultiStorePage
    {
        
        public BVApplication BVApp { get; set; }

        

        protected override void OnPreInit(System.EventArgs e)
        {
            base.OnPreInit(e);
            BVApp = BVApplication.InstantiateForDataBase(new RequestContext());

            // Determine store id        
            BVApp.CurrentStore = MerchantTribe.Commerce.Utilities.UrlHelper.ParseStoreFromUrl(System.Web.HttpContext.Current.Request.Url, BVApp.AccountServices);
            if (BVApp.CurrentStore == null)
            {
                Response.Redirect("~/storenotfound");
            }

            if (BVApp.CurrentStore.Status == MerchantTribe.Commerce.Accounts.StoreStatus.Deactivated)
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
                        this.MessageBox1.ShowInformation("Default username: admin@bvcommerce.com<br />Default Password: password");
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
                    this.BVApp.CurrentStore,
                    MerchantTribe.Commerce.Utilities.SSL.SSLRedirectTo.SSL);
            }
        }

        protected void btnLogin_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            this.MessageBox1.ClearMessage();


            string email = this.UsernameField.Text.Trim();
            string password = this.PasswordField.Text.Trim();

            string err = string.Empty;

            if ((BVApp.AccountServices.LoginAdminUser(email, password, ref err, Page.Request.RequestContext.HttpContext)))
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