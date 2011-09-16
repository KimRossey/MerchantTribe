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

namespace BVCommerce
{

    partial class BVAdmin_Configuration_Affiliates : BaseAdminPage
    {
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Affiliate Settings";
            this.CurrentTab = AdminTabType.Configuration;
            ValidateCurrentUserHasPermission(SystemPermissions.SettingsView);
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {

                this.AffiliateCommissionAmountField.Text = BVApp.CurrentStore.Settings.AffiliateCommissionAmount.ToString();
                this.AffiliateReferralDays.Text = BVApp.CurrentStore.Settings.AffiliateReferralDays.ToString();
                this.lstCommissionType.ClearSelection();
                switch (BVApp.CurrentStore.Settings.AffiliateCommissionType)
                {
                    case AffiliateCommissionType.PercentageCommission:
                    case AffiliateCommissionType.None:
                        this.lstCommissionType.Items.FindByValue("1").Selected = true;
                        break;
                    case AffiliateCommissionType.FlatRateCommission:
                        this.lstCommissionType.Items.FindByValue("2").Selected = true;
                        break;
                    default:
                        this.lstCommissionType.Items.FindByValue("1").Selected = true;
                        break;
                }
                this.AffiliateConflictModeField.ClearSelection();
                switch (BVApp.CurrentStore.Settings.AffiliateConflictMode)
                {
                    case AffiliateConflictMode.FavorOldAffiliate:
                    case AffiliateConflictMode.None:
                        this.AffiliateConflictModeField.Items.FindByValue("1").Selected = true;
                        break;
                    case AffiliateConflictMode.FavorNewAffiliate:
                        this.AffiliateConflictModeField.Items.FindByValue("2").Selected = true;
                        break;
                    default:
                        this.AffiliateConflictModeField.Items.FindByValue("1").Selected = true;
                        break;
                }

            }
        }

        protected void btnCancel_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            Response.Redirect("default.aspx");
        }

        protected void btnSave_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            if (this.Save() == true)
            {
                this.MessageBox1.ShowOk("Settings saved successfully.");
            }
        }

        private bool Save()
        {
            BVApp.CurrentStore.Settings.AffiliateCommissionAmount = decimal.Parse(this.AffiliateCommissionAmountField.Text);
            BVApp.CurrentStore.Settings.AffiliateReferralDays = int.Parse(this.AffiliateReferralDays.Text);
            int typeSelection = int.Parse(this.lstCommissionType.SelectedValue);
            BVApp.CurrentStore.Settings.AffiliateCommissionType = (AffiliateCommissionType)typeSelection;
            int conflictSelection = int.Parse(this.AffiliateConflictModeField.SelectedValue);
            BVApp.CurrentStore.Settings.AffiliateConflictMode = (AffiliateConflictMode)conflictSelection;

            return BVApp.UpdateCurrentStore();
        }

    }
}