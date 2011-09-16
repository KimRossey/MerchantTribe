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
using System.Collections.ObjectModel;

namespace BVCommerce
{

    public partial class BVAdmin_Configuration_ShippingHandling : BaseAdminPage
    {

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Shipping | Handling Settings";
            this.CurrentTab = AdminTabType.Configuration;
            ValidateCurrentUserHasPermission(SystemPermissions.SettingsView);
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {
                LoadHandlingSettings();
            }
        }

        private void LoadHandlingSettings()
        {
            this.HandlingFeeAmountTextBox.Text = BVApp.CurrentStore.Settings.HandlingAmount.ToString("c");
            this.HandlingRadioButtonList.SelectedIndex = BVApp.CurrentStore.Settings.HandlingType;
            this.NonShippingCheckBox.Checked = BVApp.CurrentStore.Settings.HandlingNonShipping;
        }

        protected void HandlingFeeAmountCustomValidator_ServerValidate(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
        {
            decimal temp;
            if (decimal.TryParse(args.Value, System.Globalization.NumberStyles.Currency,
                System.Threading.Thread.CurrentThread.CurrentUICulture, out temp))
            {
                args.IsValid = true;
            }
            else
            {
                args.IsValid = false;
            }
        }

        protected void CancelImageButton_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            Response.Redirect("~/BVAdmin/Configuration/default.aspx");
        }

        protected void SaveImageButton_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            BVApp.CurrentStore.Settings.HandlingAmount = decimal.Parse(this.HandlingFeeAmountTextBox.Text, System.Globalization.NumberStyles.Currency);
            BVApp.CurrentStore.Settings.HandlingType = this.HandlingRadioButtonList.SelectedIndex;
            BVApp.CurrentStore.Settings.HandlingNonShipping = this.NonShippingCheckBox.Checked;
            BVApp.UpdateCurrentStore();

            this.MessageBox1.ShowOk("Settings saved successfully.");
        }
    }
}