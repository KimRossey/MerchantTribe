using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BVSoftware.Commerce.Content;
using BVSoftware.Commerce.Membership;
using BVSoftware.Commerce.Storage;

namespace BVCommerce
{

    public partial class BVAdmin_Configuration_ThemesEditHeaderFooter : BaseAdminPage
    {
        protected override void OnInit(System.EventArgs e)
        {
            base.OnInit(e);
            this.PageTitle = "Edit Theme | Header and Footer";
            this.CurrentTab = AdminTabType.Configuration;
            ValidateCurrentUserHasPermission(SystemPermissions.SettingsView);
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            string themeId = Request.QueryString["id"];
            LoadTheInfo(themeId);
        }

        private void LoadTheInfo(string themeId)
        {
            this.litheader.Text = "<textarea id=\"headerhtml\" name=\"headerhtml\" style=\"width:700px;height:180px;\" wrap=\"false\">";
            this.litheader.Text += BVSoftware.Commerce.Storage.DiskStorage.ReadCustomHeader(BVApp.CurrentStore.Id, themeId);
            this.litheader.Text += "</textarea>";

            this.litFooter.Text = "<textarea id=\"footerhtml\" name=\"footerhtml\" style=\"width:700px;height:180px;\" wrap=\"false\">";
            this.litFooter.Text += BVSoftware.Commerce.Storage.DiskStorage.ReadCustomFooter(BVApp.CurrentStore.Id, themeId);
            this.litFooter.Text += "</textarea>";

            this.litIdField.Text = "<input type=\"hidden\" id=\"themeidfield\" name=\"themeidfield\" value=\"" + themeId + "\" />";
        }

        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            string themeId = Request.QueryString["id"];

            string headerhtml = Request.Form["headerhtml"];
            string footerhtml = Request.Form["footerhtml"];

            bool result = BVSoftware.Commerce.Storage.DiskStorage.WriteCustomFooter(BVApp.CurrentStore.Id, themeId, footerhtml);
            if (result)
            {
                result = BVSoftware.Commerce.Storage.DiskStorage.WriteCustomHeader(BVApp.CurrentStore.Id, themeId, headerhtml);
            }

            if (result)
            {
                this.MessageBox1.ShowOk("Changes Saved as " + System.DateTime.Now.ToLongTimeString());
            }
            else
            {
                this.MessageBox1.ShowWarning("Unable to save changes");
            }

            LoadTheInfo(themeId);
        }
    }
}