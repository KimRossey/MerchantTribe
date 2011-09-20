using System;
using System.Web;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce.Membership;
using MerchantTribe.Commerce.Content;
using MerchantTribe.Commerce;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace MerchantTribeStore
{

    partial class BVAdmin_Content_CategoryTemplates : BaseAdminPage
    {

        private StringCollection templates;
        private StringCollection templateEditors;

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Category Templates";
            this.CurrentTab = AdminTabType.Content;
            ValidateCurrentUserHasPermission(SystemPermissions.ContentView);
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {
                LoadTemplates();
            }
        }

        private void LoadTemplates()
        {
            templates = ModuleController.FindCategoryTemplates();
            templateEditors = ModuleController.FindCategoryTemplateEditors();

            this.GridView1.DataSource = templates;
            this.GridView1.DataBind();
            this.lblResults.Text = templates.Count + " Templates Found";
        }

        protected void GridView1_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string val = (string)e.Row.DataItem;
                ((Label)e.Row.FindControl("TemplateNameLabel")).Text = val;
                LinkButton EditLinkButton = (LinkButton)e.Row.FindControl("EditLinkButton");
                if (templateEditors.Contains(val))
                {
                    EditLinkButton.Visible = true;
                    EditLinkButton.CommandArgument = val;
                }
                else
                {
                    EditLinkButton.Visible = false;
                    EditLinkButton.CommandArgument = "";
                }
            }
        }

        protected void GridView1_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Edit")
            {
                Response.Redirect("CategoryTemplatesEdit.aspx?template=" + HttpUtility.UrlEncode((string)e.CommandArgument));
            }
        }
    }
}