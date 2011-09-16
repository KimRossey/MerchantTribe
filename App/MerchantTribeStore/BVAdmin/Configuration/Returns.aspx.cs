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

    partial class BVAdmin_Configuration_Returns : BaseAdminPage
    {
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Return Settings";
            this.CurrentTab = AdminTabType.Configuration;
            ValidateCurrentUserHasPermission(SystemPermissions.SettingsView);
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {

                if (BVApp.CurrentStore.Settings.AutomaticallyIssueRMANumbers)
                {
                    this.AutomaticallyIssueRMACheckBoxList.SelectedValue = "1";
                }
                else
                {
                    this.AutomaticallyIssueRMACheckBoxList.SelectedValue = "0";
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
            BVApp.CurrentStore.Settings.AutomaticallyIssueRMANumbers = (AutomaticallyIssueRMACheckBoxList.SelectedValue == "1");
            return BVApp.UpdateCurrentStore();            
        }

    }
}