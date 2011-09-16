using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.ObjectModel;
using BVSoftware.Commerce.Content;
using BVSoftware.Commerce.Membership;
using BVSoftware.Commerce.Storage;

namespace BVCommerce
{

    public partial class BVAdmin_Configuration_ThemesEditCSS : BaseAdminPage
    {
        protected override void OnInit(System.EventArgs e)
        {
            base.OnInit(e);
            this.PageTitle = "Edit Theme | CSS";
            this.CurrentTab = AdminTabType.Configuration;
            ValidateCurrentUserHasPermission(SystemPermissions.SettingsView);
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {
                string themeId = Request.QueryString["id"];
                LoadTheInfo(themeId);
                this.themeidfield.Value = themeId;
            }

        }

        private void LoadTheInfo(string themeId)
        {
            this.litEditor.Text = "<textarea id=\"EditForm\" name=\"EditForm\" style=\"width:700px;height:400px;overflow:auto;\" wrap=\"off\">";
            this.litEditor.Text += BVApp.ThemeManager().CurrentStyleSheetContent(themeId);
            this.litEditor.Text += "</textarea>";
        }

        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            this.MessageBox1.ClearMessage();
            SaveCss();
        }

        private void SaveCss()
        {
            string themeid = Request.QueryString["id"];
            string css = Request.Form["EditForm"];
            bool result = BVApp.ThemeManager().UpdateStyleSheet(themeid, css);
            if (result)
            {
                this.MessageBox1.ShowOk("Changes Saved!");
            }
            else
            {
                this.MessageBox1.ShowWarning("Unable to save changes!");
            }
            LoadTheInfo(themeid);
        }
    }
}