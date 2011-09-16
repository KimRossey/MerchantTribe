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

    partial class BVAdmin_Content_CustomUrl : BaseAdminPage
    {

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Custom Urls";
            this.CurrentTab = AdminTabType.Content;
            ValidateCurrentUserHasPermission(SystemPermissions.ContentView);
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {
                GridView1.PageSize = WebAppSettings.RowsPerPage;
                LoadUrls();
            }
        }

        private void LoadUrls()
        {
            this.GridView1.DataBind();
        }

        protected void GridView1_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            msg.ClearMessage();
            string bvin = (string)GridView1.DataKeys[e.RowIndex].Value;
            if (BVApp.ContentServices.CustomUrls.Delete(bvin) == false)
            {
                this.msg.ShowWarning("Unable to delete this custom Url.");
            }

            LoadUrls();
            e.Cancel = true;
        }

        protected void btnNew_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            msg.ClearMessage();
            Response.Redirect("CustomUrl_Edit.aspx");
        }

        protected void GridView1_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
        {
            string bvin = (string)GridView1.DataKeys[e.NewEditIndex].Value;
            Response.Redirect("CustomUrl_Edit.aspx?id=" + bvin);
        }

        protected void ObjectDataSource1_Selecting(object sender, System.Web.UI.WebControls.ObjectDataSourceSelectingEventArgs e)
        {
            if (e.ExecutingSelectCount)
            {
                e.InputParameters["rowCount"] = HttpContext.Current.Items["RowCount"];
                this.lblResults.Text = (int)HttpContext.Current.Items["RowCount"] + " Urls found";
                HttpContext.Current.Items["RowCount"] = null;
            }
        }

        protected void ObjectDataSource1_Selected(object sender, System.Web.UI.WebControls.ObjectDataSourceStatusEventArgs e)
        {
            if (e.OutputParameters["RowCount"] != null)
            {
                HttpContext.Current.Items["RowCount"] = e.OutputParameters["RowCount"];
            }
        }
    }

}