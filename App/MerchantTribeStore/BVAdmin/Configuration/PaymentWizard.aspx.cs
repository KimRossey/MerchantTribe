using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Text;
using BVSoftware.Commerce;
using BVSoftware.Commerce.Accounts;
using BVSoftware.Commerce.BusinessRules;
using BVSoftware.Commerce.Catalog;
using BVSoftware.Commerce.Contacts;
using BVSoftware.Commerce.Content;
using BVSoftware.Commerce.Membership;
using BVSoftware.Commerce.Metrics;
using BVSoftware.Commerce.Orders;
using BVSoftware.Commerce.Payment;
using BVSoftware.Commerce.Shipping;
using BVSoftware.Commerce.Taxes;
using BVSoftware.Commerce.Utilities;

using System.Collections.Generic;

namespace BVCommerce
{

    partial class BVAdmin_Configuration_PaymentWizard : BaseAdminPage
    {
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Payment Setup Wizard";
            this.CurrentTab = AdminTabType.Configuration;
            ValidateCurrentUserHasPermission(SystemPermissions.SettingsView);
        }

        protected void lnkWithPayPal_Click(object sender, System.EventArgs e)
        {
            this.multiview1.SetActiveView(this.step2);
        }

        protected void lnkNoPayPal_Click(object sender, System.EventArgs e)
        {
            this.multiview1.SetActiveView(this.step2b);
        }

        protected void lnkBackFrom2a_Click(object sender, System.EventArgs e)
        {
            this.multiview1.SetActiveView(this.step1);
        }

        protected void lnkBackFrom2_Click(object sender, System.EventArgs e)
        {
            this.multiview1.SetActiveView(this.step1);
        }

        protected void lnkBackFrom3b_Click(object sender, System.EventArgs e)
        {
            this.multiview1.SetActiveView(this.step2b);
        }

        protected void lnkBackFrom3a_Click(object sender, System.EventArgs e)
        {
            this.multiview1.SetActiveView(this.step2);
        }

        protected void btnPayPalStandard_Click(object sender, System.EventArgs e)
        {
            this.multiview1.SetActiveView(this.step3a);
        }

        protected void btnPayPayPro_Click(object sender, System.EventArgs e)
        {
            this.multiview1.SetActiveView(this.step3a);
        }

        protected void btnSaveGateway_Click(object sender, System.EventArgs e)
        {
            this.multiview1.SetActiveView(this.step3b);
        }

        protected void btnPayPalConfig_Click(object sender, System.EventArgs e)
        {
            this.multiview1.SetActiveView(step4);
        }

        protected void btnCCConfig_Click(object sender, System.EventArgs e)
        {
            this.multiview1.SetActiveView(step4);
        }
    }
}