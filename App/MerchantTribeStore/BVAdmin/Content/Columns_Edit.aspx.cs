using System;
using System.Web;
using System.Web.UI.WebControls;
using BVSoftware.Commerce.Membership;
using BVSoftware.Commerce.Content;
using BVSoftware.Commerce;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

using System.IO;

namespace BVCommerce
{

    partial class BVAdmin_Content_Columns_Edit : BaseAdminPage
    {
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Edit Content Column";
            this.CurrentTab = AdminTabType.Content;
            ValidateCurrentUserHasPermission(SystemPermissions.ContentView);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {
                PopulateAdvancedOptions();
                if (Request.QueryString["id"] != null)
                {
                    ContentColumnEditor.ColumnId = Request.QueryString["id"];
                }
            }
        }

        private void PopulateAdvancedOptions()
        {
            this.CopyToList.DataSource = BVApp.ContentServices.Columns.FindAll();
            this.CopyToList.DataTextField = "DisplayName";
            this.CopyToList.DataValueField = "bvin";
            this.CopyToList.DataBind();
        }

        protected void btnOk_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            Response.Redirect("Columns.aspx");
        }

        protected void btnCopyBlocks_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            this.msg.ClearMessage();

            ContentColumn c = BVApp.ContentServices.Columns.Find(ContentColumnEditor.ColumnId);
            if (c != null)
            {
                string destinationColumnId = this.CopyToList.SelectedValue;
                foreach (ContentBlock b in c.Blocks)
                {
                    BVApp.ContentServices.Columns.CopyBlockToColumn(b.Bvin, destinationColumnId);
                }                
                this.msg.ShowOk("Column Copied");
            }
            else
            {
                this.msg.ShowError("Copy failed. Unknown Error.");
            }
            ContentColumnEditor.LoadColumn();
        }

        protected void btnClone_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            this.msg.ClearMessage();

            if (this.CloneNameField.Text.Trim().Length < 1)
            {
                msg.ShowWarning("Please enter a name first.");
            }
            else
            {
                ContentColumn clone = BVApp.ContentServices.Columns.Clone(ContentColumnEditor.ColumnId, this.CloneNameField.Text.Trim());
                this.msg.ShowOk("Column Copied");                
            }
            ContentColumnEditor.LoadColumn();
        }
    }
}