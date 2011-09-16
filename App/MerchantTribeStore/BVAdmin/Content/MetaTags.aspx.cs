using System;
using System.Web;
using System.Web.UI.WebControls;
using BVSoftware.Commerce.Membership;
using BVSoftware.Commerce.Content;
using BVSoftware.Commerce;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace BVCommerce
{

    partial class BVAdmin_Content_MetaTags : BaseAdminPage
    {

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {
                this.MetaKeywordsField.Text = BVApp.CurrentStore.Settings.MetaKeywords;
                this.MetaDescriptionField.Text = BVApp.CurrentStore.Settings.MetaDescription;
            }
        }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Meta Tags";
            this.CurrentTab = AdminTabType.Content;
            ValidateCurrentUserHasPermission(SystemPermissions.ContentView);
        }

        protected void btnCancel_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            Response.Redirect("default.aspx");
        }

        protected void btnSave_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            if (this.Save() == true)
            {
                MessageBox1.ShowOk("Meta Tags Saved Successfully.");
            }
            else
            {
                MessageBox1.ShowError("Error Occurred While Saving. Please Check Event Log.");
            }
        }

        private bool Save()
        {

            BVApp.CurrentStore.Settings.MetaKeywords = this.MetaKeywordsField.Text.Trim();
            BVApp.CurrentStore.Settings.MetaDescription = this.MetaDescriptionField.Text.Trim();

            return BVApp.UpdateCurrentStore();
        }

    }
}