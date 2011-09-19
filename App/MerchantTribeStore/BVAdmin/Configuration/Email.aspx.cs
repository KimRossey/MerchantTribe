using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Text;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Accounts;
using MerchantTribe.Commerce.BusinessRules;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Contacts;
using MerchantTribe.Commerce.Content;
using MerchantTribe.Commerce.Membership;
using MerchantTribe.Commerce.Metrics;
using MerchantTribe.Commerce.Orders;
using MerchantTribe.Commerce.Payment;
using MerchantTribe.Commerce.Shipping;
using MerchantTribe.Commerce.Taxes;
using MerchantTribe.Commerce.Utilities;

namespace BVCommerce
{

    partial class BVAdmin_Configuration_Email : BaseAdminPage
    {
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Email Addresses";
            this.CurrentTab = AdminTabType.Configuration;
            ValidateCurrentUserHasPermission(SystemPermissions.SettingsView);
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {
                this.ContactEmailField.Text = BVApp.CurrentStore.Settings.MailServer.EmailForGeneral;
                this.OrderNotificationEmailField.Text = BVApp.CurrentStore.Settings.MailServer.EmailForNewOrder;
            }
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
            BVApp.CurrentStore.Settings.MailServer.EmailForGeneral = this.ContactEmailField.Text.Trim();
            BVApp.CurrentStore.Settings.MailServer.EmailForNewOrder = this.OrderNotificationEmailField.Text.Trim();
            return BVApp.UpdateCurrentStore();            
        }
    }

}