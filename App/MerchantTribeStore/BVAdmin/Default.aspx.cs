using System;
using BVSoftware.Commerce;

namespace BVCommerce
{

    partial class BVAdmin_Default : BaseAdminPage
    {

        public override bool RequiresSSL
        {
            get
            {
                return true;
            }
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (WebAppSettings.IsIndividualMode)
            {
                // Simple pci check for default admin username
                if (BVApp.CurrentRequestContext.CurrentAdministrator.Email == "admin@bvcommerce.com") Response.Redirect("ChangeEmail.aspx?pci=1");
            }

            this.pnlGettingStarted.Visible = !BVApp.CurrentStore.Settings.HideGettingStarted;
            ShowFreeMessage();
        }

        protected override void OnPreInit(System.EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Dashboard";
            this.CurrentTab = AdminTabType.Dashboard;
        }

        protected void lnkHideGettingStarted_Click(object sender, EventArgs e)
        {
            BVApp.CurrentStore.Settings.HideGettingStarted = true;
            BVApp.UpdateCurrentStore();
            this.pnlGettingStarted.Visible = false;
        }

        private void ShowFreeMessage()
        {
            if (BVApp.CurrentStore.PlanId == 0)
            {
                this.litFreePlan.Text = "<div class=\"flash-message-info\">Your store is on the Free plan. <a href=\"ChangePlan.aspx\">Upgrade Your Store</a> to support more products and features.</div>";
            }
        }
    }
}