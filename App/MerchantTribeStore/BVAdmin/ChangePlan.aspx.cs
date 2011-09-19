using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce.Accounts;
using MerchantTribe.Billing;

namespace BVCommerce
{

    public partial class BVAdmin_ChangePlan : BaseAdminPage
    {
        protected override void OnPreInit(System.EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Change Plan";
            this.CurrentTab = AdminTabType.Dashboard;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {
                UserAccount u = GetCorrectUser();
                PopulatePage(u);
                if (Request.QueryString["ok"] != null)
                {
                    this.MessageBox1.ShowOk("Plan Changed!");
                }
            }
        }

        private UserAccount GetCorrectUser()
        {
            UserAccount u = CurrentUser;

            if (u != null)
            {
                if (u.Status == UserAccountStatus.SuperUser)
                {
                    // don't use current user, get the owner of the store instead
                    List<UserAccount> users = BVApp.AccountServices.FindAdminUsersByStoreId(BVApp.CurrentStore.Id);
                    if (users != null)
                    {
                        if (users.Count > 0)
                        {
                            return users[0];
                        }
                    }
                }
            }

            return u;
        }

        private void PopulatePage(UserAccount u)
        {
            SetCurrentStore();
            LoadBilling(u);
        }

        private void SetCurrentStore()
        {
            switch (BVApp.CurrentStore.PlanId)
            {
                case 0:
                    this.btnFree.Text = "Current Plan";
                    break;
                case 1:
                    this.btnBasic.Text = "Current Plan";
                    break;
                case 2:
                    this.btnPlus.Text = "Current Plan";
                    break;
                case 3:
                    this.btnPremium.Text = "Current Plan";
                    break;
                case 99:
                    this.btnMax.Text = "Current Plan";
                    break;
            }
        }

        private void LoadBilling(UserAccount u)
        {
            MerchantTribe.Billing.Service svc = new MerchantTribe.Billing.Service(MerchantTribe.Commerce.WebAppSettings.ApplicationConnectionString);
            BillingAccount act = svc.Accounts.FindOrCreate(u.Email);
            if (act != null)
            {

                this.lblCardOnFile.Text = act.CreditCard.CardTypeName + "-" + act.CreditCard.CardNumberLast4Digits + "<br />";
                this.lblCardOnFile.Text += "Expires: " + act.CreditCard.ExpirationMonth + "/" + act.CreditCard.ExpirationYear + "<br />";
                this.lblCardOnFile.Text += "Billing Zip: " + act.BillingZipCode;

                if (act.CreditCard.CardNumber == "4111111111111111") this.lblCardOnFile.Text = "&nbsp;<br /><b>No Card On File.</b> Add One before choosing a plan!<br />&nbsp;";
            }
        }
        protected void btnFree_Click(object sender, EventArgs e)
        {
            if (CheckMax(0))
            {
                BVApp.AccountServices.ChangePlan(BVApp.CurrentStore.Id, GetCorrectUser().Id, 0, BVApp);
                Response.Redirect("ChangePlan.aspx?ok=1");
            }
        }
        protected void btnBasic_Click(object sender, EventArgs e)
        {
            if (CheckMax(1))
            {
                BVApp.AccountServices.ChangePlan(BVApp.CurrentStore.Id, GetCorrectUser().Id, 1, BVApp);
                Response.Redirect("ChangePlan.aspx?ok=1");
            }
        }
        protected void btnPlus_Click(object sender, EventArgs e)
        {
            if (CheckMax(2))
            {
                BVApp.AccountServices.ChangePlan(BVApp.CurrentStore.Id, GetCorrectUser().Id, 2, BVApp);
                Response.Redirect("ChangePlan.aspx?ok=1");
            }
        }
        protected void btnPremium_Click(object sender, EventArgs e)
        {
            if (CheckMax(3))
            {
                BVApp.AccountServices.ChangePlan(BVApp.CurrentStore.Id, GetCorrectUser().Id, 3, BVApp);
                Response.Redirect("ChangePlan.aspx?ok=1");
            }
        }
        protected void btnMax_Click(object sender, EventArgs e)
        {
            if (CheckMax(99))
            {
                BVApp.AccountServices.ChangePlan(BVApp.CurrentStore.Id, GetCorrectUser().Id, 99, BVApp);
                Response.Redirect("ChangePlan.aspx?ok=1");
            }
        }

        private bool CheckMax(int newPlan)
        {
            int current = BVApp.CatalogServices.Products.FindAllCount();
            HostedPlan p = HostedPlan.FindById(newPlan);
            if (current > p.MaxProducts)
            {
                this.MessageBox1.ShowWarning("You have too many products to downgrade to that plan. Remove some products first.");
                return false;
            }

            return true;
        }
    }
}