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

    partial class BVAdmin_Content_StoreClosed : BaseAdminPage
    {

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Store Closed Content";
            this.CurrentTab = AdminTabType.Content;
            ValidateCurrentUserHasPermission(SystemPermissions.ContentView);
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {

                this.ContentField.Focus();
                LoadData();

            }
        }

        private void LoadData()
        {
            this.ContentField.PreTransformText = BVApp.CurrentStore.Settings.StoreClosedDescriptionPreTransform;

            this.ContentField.Text = BVApp.CurrentStore.Settings.StoreClosedDescription;
            if (this.ContentField.SupportsTransform)
            {
                if (BVApp.CurrentStore.Settings.StoreClosedDescriptionPreTransform.Length > 0)
                {
                    this.ContentField.Text = BVApp.CurrentStore.Settings.StoreClosedDescriptionPreTransform;
                }
            }
        }

        protected void btnSaveChanges_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            if (this.Save() == true)
            {
                Response.Redirect("default.aspx");
            }
        }

        protected void btnCancel_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            Response.Redirect("Default.aspx");
        }

        private bool Save()
        {
            BVApp.CurrentStore.Settings.StoreClosedDescription = this.ContentField.Text;
            BVApp.CurrentStore.Settings.StoreClosedDescriptionPreTransform = this.ContentField.PreTransformText;
            return BVApp.UpdateCurrentStore();
        }


    }

}