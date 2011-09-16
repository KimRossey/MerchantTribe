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

    partial class BVAdmin_Content_CategoryTemplatesEdit : BaseAdminPage
    {
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Category Templates Edit";
            this.CurrentTab = AdminTabType.Content;
            ValidateCurrentUserHasPermission(SystemPermissions.ContentView);
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {
                if (Request.QueryString["template"] == null)
                {
                    Response.Redirect("CategoryTemplates.aspx");
                }
                else
                {
                    ViewState["template"] = Request.QueryString["template"];
                }
            }
            LoadCategoryTemplate();
        }

        private void LoadCategoryTemplate()
        {
            CategoryEditorTemplate categoryTemplateEditor = ModuleController.LoadCategoryEditor((string)ViewState["template"], this);
            categoryTemplateEditor.ID = "ChoiceEditControl1";
            categoryTemplateEditor.EditingComplete += FinishedEditing;
            CategoryEditorPanel.Controls.Add(categoryTemplateEditor);
        }

        protected void FinishedEditing(object sender, BVModuleEventArgs e)
        {
            Response.Redirect("CategoryTemplates.aspx");
        }
    }
}