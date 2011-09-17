using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BVCommerce.Helpers;
using BVSoftware.Commerce.Accounts;

namespace BVCommerce
{

    public partial class signup_register : BaseSignupPage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.Title = "Sign Up for BV Commerce Hosted Ecommerce Service";
            string id = (string)Page.RouteData.Values["id"];

            this.submitbutton.Enabled = this.chkAgree.Checked;

            if (!Page.IsPostBack)
            {
                // Populate Credit Card Years
                for (int i = DateTime.Now.Year; i < DateTime.Now.AddYears(10).Year; i++)
                {
                    this.expyear.Items.Add(new ListItem(i.ToString(), i.ToString()));
                }

                app.RegisterStoreData data = new app.RegisterStoreData();
                SetPlanFromName(id, data);
                AddPlanDetails(data);
                RenderData(data);


                if (PreviousPage != null)
                {
                    ProcessHomePageSignup();
                }
            }
        }

        private void ProcessHomePageSignup()
        {
            // Coming from homepage
            ContentPlaceHolder oldHolder = (ContentPlaceHolder)PreviousPage.Controls[0].FindControl("MainContent");
            if (oldHolder != null)
            {
                HiddenField hiddenAction = (HiddenField)oldHolder.FindControl("HiddenAction");
                if (hiddenAction != null)
                {
                    TextBox oldEmail = (TextBox)oldHolder.FindControl("EmailField");
                    if (oldEmail != null) this.EmailField.Text = oldEmail.Text;
                    TextBox oldPassword = (TextBox)oldHolder.FindControl("Password");
                    string pass = oldPassword.Text;
                    TextBox oldStoreName = (TextBox)oldHolder.FindControl("storename");
                    if (oldStoreName != null) this.storename.Text = oldStoreName.Text;

                    if (hiddenAction.Value == "freesignup")
                    {
                        this.chkAgree.Checked = true;
                        this.submitbutton.Enabled = true;
                        ProcessSignup(pass);
                    }
                }
            }
        }
        private void SetPlanFromName(string id, app.RegisterStoreData data)
        {
            if (id != string.Empty)
            {
                switch (id.Trim().ToLower())
                {
                    case "starter":
                        data.plan = 0;
                        break;
                    case "basic":
                        data.plan = 1;
                        this.creditcarddiv.Visible = true;
                        break;
                    case "plus":
                        data.plan = 2;
                        this.creditcarddiv.Visible = true;
                        break;
                    case "premium":
                        data.plan = 3;
                        this.creditcarddiv.Visible = true;
                        break;
                    case "max":
                        data.plan = 99;
                        this.creditcarddiv.Visible = true;
                        break;
                    default:
                        data.plan = 0;
                        break;
                }
            }
        }

        private void AddPlanDetails(app.RegisterStoreData data)
        {
            string PlanDescription = "You are signing up for the <strong>{0}</strong> plan. {2}Your card will be charged <strong>{1}</strong> today and each month after on this day until you cancel.";
            DateTime expires = MerchantTribe.Web.Dates.MaxOutTime(DateTime.Now).AddMonths(1);

            HostedPlan thePlan = HostedPlan.FindById(data.plan);
            if (thePlan != null)
            {
                string PayPalLead = string.Empty;
                PayPalLead = BVSoftware.Commerce.SessionManager.GetCookieString("PayPalLead");

                if (thePlan.Id == 0)
                {
                    data.plandetails = String.Format(PlanDescription, "Free Plan", "$0", "Free plans are never charged. ");
                }
                else
                {
                    if (PayPalLead != string.Empty)
                    {
                        data.plandetails = String.Format(PlanDescription, thePlan.Name, 49.ToString("c"), "");
                    }
                    else
                    {
                        data.plandetails = String.Format(PlanDescription, thePlan.Name, thePlan.Rate.ToString("c"), "");
                    }
                }
            }
        }

        private void RenderData(app.RegisterStoreData data)
        {
            this.EmailField.Text = data.email;
            this.Password.Text = data.password;
            this.storename.Text = data.storename;
            this.litPlanDetails.Text = data.plandetails;
        }

        protected void submitbutton_Click(object sender, EventArgs e)
        {
            ProcessSignup(this.Password.Text.Trim());
        }


        private void RenderError(MerchantTribe.Web.Validation.RuleViolation v)
        {
            RenderError(v.ControlName, v.ErrorMessage);
        }
        private void RenderError(string control, string message)
        {
            this.litValidation.Text += "<div class=\"flash-message-validation\">" + message + "</div>";
            AssignCssError(control);
        }

        private void ClearErrorCss()
        {
            this.EmailField.CssClass = "";
            this.Password.CssClass = "";
            this.storename.CssClass = "";
            this.cardnumber.CssClass = "";
            this.cardholder.CssClass = "";
            this.billingzipcode.CssClass = "";
            this.expyear.CssClass = "";
            this.expmonth.CssClass = "";
        }

        private void AssignCssError(string controlName)
        {
            string css = "input-validation-error";

            switch (controlName)
            {
                case "storename":
                    this.storename.CssClass = css;
                    break;
                case "email":
                    this.EmailField.CssClass = css;
                    break;
                case "password":
                    this.Password.CssClass = css;
                    break;
                case "cardholder":
                    this.cardholder.CssClass = css;
                    break;
                case "cardnumber":
                    this.cardnumber.CssClass = css;
                    break;
                case "billingzipcode":
                    this.billingzipcode.CssClass = css;
                    break;
                case "expyear":
                    this.expyear.CssClass = css;
                    break;
                case "expmonth":
                    this.expmonth.CssClass = css;
                    break;
            }
        }

        private void ProcessSignup(string pass)
        {
            ClearErrorCss();

            string id = (string)Page.RouteData.Values["id"];
            app.RegisterStoreData data = new app.RegisterStoreData();
            SetPlanFromName(id, data);
            AddPlanDetails(data);
            data.email = this.EmailField.Text.Trim();
            data.password = pass; // this.Password.Text.Trim();
            data.storename = this.storename.Text.Trim();

            bool storeOkay = false;

            if (data.plan == 0)
            {
                // Fake CC information so that signup is easier
                data.cardholder = data.email;
                data.cardnumber = "4111111111111111";
                data.billingzipcode = "00000";
                data.expyear = DateTime.Now.AddYears(1).Year;
                data.expmonth = DateTime.Now.Month;
            }
            else
            {
                // Paid Plan
                data.cardholder = this.cardholder.Text.Trim();
                data.cardnumber = this.cardnumber.Text.Trim();
                data.billingzipcode = this.billingzipcode.Text.Trim();
                data.expyear = int.Parse(this.expyear.SelectedItem.Value);
                data.expmonth = int.Parse(this.expmonth.SelectedItem.Value);
            }

            BVSoftware.Commerce.Accounts.Store testStore = new BVSoftware.Commerce.Accounts.Store();
            testStore.StoreName = data.storename;
            if (!testStore.IsValid())
            {
                foreach (MerchantTribe.Web.Validation.RuleViolation v in testStore.GetRuleViolations())
                {
                    RenderError(v); //ModelState.AddModelError(v.ControlName, v.ErrorMessage);
                }
            }
            else
            {
                if (BVApp.AccountServices.StoreNameExists(testStore.StoreName))
                {
                    RenderError("storename", "A store with that name already exists. Choose another name and try again.");
                }
                else
                {
                    storeOkay = true;
                }
            }

            // Check credit card number
            bool cardOkay = true;
            if (!BVSoftware.Payment.CardValidator.IsCardNumberValid(data.cardnumber))
            {
                cardOkay = false;
                RenderError("cardnumber", "Please enter a valid credit card number");
            }
            if (data.cardholder.Trim().Length < 1)
            {
                cardOkay = false;
                RenderError("cardholder", "Please enter the name on your credit card.");
            }
            if (data.billingzipcode.Trim().Length < 5)
            {
                cardOkay = false;
                RenderError("billingzipcode", "Please enter the billing zip code for your credit card.");
            }

            UserAccount u = BVApp.AccountServices.AdminUsers.FindByEmail(data.email);

            if (u == null)
            {
                u = new BVSoftware.Commerce.Accounts.UserAccount();
            }

            bool userOk = false;
            if (u.IsValid() && (u.Email == data.email))
            {
                if (u.DoesPasswordMatch(data.password))
                {
                    userOk = true;
                }
                else
                {
                    RenderError("email", "A user account with that email address already exists. If it's your account make sure you enter the exact same password as you usually use.");
                }
            }
            else
            {
                u = new BVSoftware.Commerce.Accounts.UserAccount();
                u.Email = data.email;
                u.HashedPassword = data.password;

                if (u.IsValid())
                {
                    u.Status = BVSoftware.Commerce.Accounts.UserAccountStatus.Active;
                    u.DateCreated = DateTime.UtcNow;
                    userOk = BVApp.AccountServices.AdminUsers.Create(u);
                    u = BVApp.AccountServices.AdminUsers.FindByEmail(u.Email);
                }
                else
                {
                    foreach (MerchantTribe.Web.Validation.RuleViolation v in u.GetRuleViolations())
                    {
                        RenderError(v.ControlName, v.ErrorMessage);
                    }
                }
            }

            if (userOk && storeOkay && cardOkay)
            {
                try
                {
                    BVSoftware.Billing.BillingAccount billingAccount = new BVSoftware.Billing.BillingAccount();
                    billingAccount.Email = u.Email;
                    billingAccount.BillingZipCode = data.billingzipcode;
                    billingAccount.CreditCard.CardNumber = data.cardnumber;
                    billingAccount.CreditCard.ExpirationMonth = data.expmonth;
                    billingAccount.CreditCard.ExpirationYear = data.expyear;
                    billingAccount.CreditCard.CardHolderName = data.cardholder;

                    bool isPayPalLead = false;
                    if (BVSoftware.Commerce.SessionManager.GetCookieString("PayPalLead") != string.Empty)
                    {
                        isPayPalLead = true;
                    }

                    decimal rate = 0;
                    HostedPlan thePlan = HostedPlan.FindById(data.plan);
                    if (thePlan != null)
                    {
                        rate = thePlan.Rate;
                        if (isPayPalLead)
                        {
                            rate = 49;
                        }
                    }

                    BVSoftware.Commerce.Accounts.Store s = BVApp.AccountServices.CreateAndSetupStore(data.storename,
                                                                u.Id,
                                                                data.storename + " store for " + data.email,
                                                                data.plan,
                                                                rate,
                                                                billingAccount);
                    if (s != null)
                    {
                        if (isPayPalLead)
                        {
                            s.Settings.LeadSource = "PayPalOffer";
                            BVApp.AccountServices.Stores.Update(s);
                        }

                        string e = MerchantTribe.Web.Cryptography.Base64.ConvertStringToBase64(u.Email);
                        string st = MerchantTribe.Web.Cryptography.Base64.ConvertStringToBase64(s.StoreName);

                        Response.Redirect("/signup/ProcessSignUp?e=" + e + "&s=" + st);

                        //this.completeemail.Text = u.Email;
                        //this.completestorelink.Text = "<a href=\"" + s.RootUrl() + "\">" + s.RootUrl() + "</a>";
                        //this.completestorelinkadmin.Text = "<a href=\"" + s.RootUrlSecure() + "bvadmin\">" + s.RootUrlSecure() + "bvadmin</a>";
                        //this.completebiglogin.Text = "<a href=\"" + s.RootUrlSecure() + "account/login?wizard=1&username=" + u.Email + "\">Next Step &raquo; Choose a Theme</a>";
                        //this.pnlComplete.Visible = true;
                        //this.pnlMain.Visible = false;                                                                                
                    }
                }
                catch (BVSoftware.Commerce.Accounts.CreateStoreException cex)
                {
                    RenderError("storename", cex.Message);
                }
            }

        }
    }
}