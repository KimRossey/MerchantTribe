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