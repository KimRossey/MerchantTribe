using System.Web.UI;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Content;
using MerchantTribe.Commerce.Membership;

namespace BVCommerce
{

    partial class BVModules_Controls_LoginControl : MerchantTribe.Commerce.Content.BVUserControl
    {

        //public bool HideIfLoggedIn
        //{
        //    get
        //    {
        //        object obj = ViewState["HideIfLoggedIn"];
        //        if (obj != null)
        //        {
        //            return (bool)obj;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    set { ViewState["HideIfLoggedIn"] = value; }
        //}      

        public bool CheckoutMode
        {
            get { return bool.Parse(this.CheckoutModeField.Value); }
            set { this.CheckoutModeField.Value = value.ToString(); }
        }

        public delegate void LoginCompletedDelegate(object sender, MerchantTribe.Commerce.Controls.LoginCompleteEventArgs args);
        public event LoginCompletedDelegate LoginCompleted;

        protected override void OnInit(System.EventArgs e)
        {
            base.OnInit(e);
            this.btnLogin.ImageUrl = this.MyPage.MTApp.ThemeManager().ButtonUrl("login", Request.IsSecureConnection);
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            //if (HideIfLoggedIn)
            //{
            //    if (SessionManager.IsUserAuthenticated)
            //    {
            //        this.Visible = false;
            //    }
            //    else
            //    {
            //        this.Visible = true;
            //    }
            //}

            if (WebAppSettings.RememberUsers)
            {
                trRememberMe.Visible = true;
                RememberMeCheckBox.Text = SiteTerms.GetTerm(SiteTermIds.RememberUser);
            }
            else
            {
                trRememberMe.Visible = false;
            }


            if (!Page.IsPostBack)
            {
                this.btnLogin.ImageUrl = MyPage.MTApp.ThemeManager().ButtonUrl("login", Request.IsSecureConnection);

                if (Request.QueryString["ReturnURL"] != null)
                {
                    this.RedirectToField.Value = Request.QueryString["ReturnURL"];
                }

                if (Request.QueryString["username"] != null)
                {
                    UsernameField.Text = Request.QueryString["username"];
                }

                this.UsernameField.Focus();

                if (WebAppSettings.RememberUsers == true)
                {
                    string uid = SessionManager.GetCookieString(WebAppSettings.CustomerIdCookieName);
                    if (uid != string.Empty)
                    {
                        CustomerAccount u = MyPage.MTApp.MembershipServices.Customers.Find(uid);
                        if (u != null)
                        {
                            this.UsernameField.Text = u.Email;
                            this.PasswordField.Focus();
                        }
                    }
                }

            }
        }

        protected void btnLogin_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            if (Page.IsValid)
            {

                string errorMessage = string.Empty;
                string userId  = string.Empty;
                if (MyPage.MTApp.MembershipServices.LoginCustomer(this.UsernameField.Text.Trim(),
                                        this.PasswordField.Text.Trim(),
                                        ref errorMessage,
                                        this.Request.RequestContext.HttpContext, 
                                        ref userId))
                {
                    MerchantTribe.Commerce.Orders.Order cart = SessionManager.CurrentShoppingCart(MyPage.MTApp.OrderServices);
                    if (cart != null && !string.IsNullOrEmpty(cart.bvin))
                    {
                        cart.UserEmail = this.UsernameField.Text.Trim();
                        cart.UserID = userId;
                        MyPage.MTApp.CalculateOrderAndSave(cart);
                        SessionManager.SaveOrderCookies(cart);
                    }

                    MerchantTribe.Commerce.Controls.LoginCompleteEventArgs args = new MerchantTribe.Commerce.Controls.LoginCompleteEventArgs();
                    args.UserId = userId;
                    args.UserEmail = this.UsernameField.Text.Trim();
                    if (LoginCompleted != null)
                    {
                        LoginCompleted(this, args);
                    }

                }
                else
                {
                    string errorMessage2 = string.Empty;
                    // Failed to Login as Customer, Try admin account
                    if (MyPage.MTApp.AccountServices.LoginAdminUser(this.UsernameField.Text.Trim(), this.PasswordField.Text.Trim(), 
                                                                               ref errorMessage2, this.Request.RequestContext.HttpContext))
                    {
                        Response.Redirect("~/bvadmin"); //System.IO.Path.Combine(MyPage.CurrentStore.RootUrlSecure(), "bvadmin"));                        
                    }
                    this.MessageBox1.ShowWarning(errorMessage);
                }
               
               
            }
        }

        protected void btnForgotPassword_Click(object sender, System.EventArgs e)
        {
            string c = "0";
            if (CheckoutMode) c = "1";
                                              
            string destination = Page.GetRouteUrl("forgotpassword-route", new { email = this.UsernameField.Text, checkout = c });

            if (destination == null) destination = string.Empty;
            if (destination == string.Empty) destination = "~/forgotpassword/@/" + c;

            Response.Redirect(destination);                                           
        }

    }
}