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

    partial class BVAdmin_Configuration_ProductReviews : BaseAdminPage
    {
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Product Review Settings";
            this.CurrentTab = AdminTabType.Configuration;
            ValidateCurrentUserHasPermission(SystemPermissions.SettingsView);
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {

                this.ProductReviewCountField.Text = BVApp.CurrentStore.Settings.ProductReviewCount.ToString();
                this.chkProductReviewModerate.Checked = BVApp.CurrentStore.Settings.ProductReviewModerate;
                this.chkProductReviewShowRating.Checked = BVApp.CurrentStore.Settings.ProductReviewShowRating;

            }
        }

        protected void btnCancel_Click(System.Object sender, System.Web.UI.ImageClickEventArgs e)
        {
            Response.Redirect("default.aspx");
        }

        protected void btnSave_Click(System.Object sender, System.Web.UI.ImageClickEventArgs e)
        {
            msg.ClearMessage();


            BVApp.CurrentStore.Settings.ProductReviewCount = int.Parse(this.ProductReviewCountField.Text.Trim());
            BVApp.CurrentStore.Settings.ProductReviewModerate = this.chkProductReviewModerate.Checked;
            BVApp.CurrentStore.Settings.ProductReviewShowRating = this.chkProductReviewShowRating.Checked;
            BVApp.UpdateCurrentStore();

            this.msg.ShowOk("Settings saved successfully.");

        }


    }
}