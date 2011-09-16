using System;
using System.Text;
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

    public partial class BVAdmin_Configuration_ThemesEditButtons : BaseAdminPage
    {
        protected override void OnInit(System.EventArgs e)
        {
            base.OnInit(e);
            this.PageTitle = "Edit Theme | Buttons";
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
            }

        }

        void LoadTheInfo(string themeid)
        {
            this.litIdField.Text = "<input type=\"hidden\" id=\"themeidfield\" name=\"themeidfield\" value=\"" + themeid + "\" />";

            StringBuilder sb = new StringBuilder();

            sb.Append("<ul class=\"removablelist\">");

            List<BVSoftware.Commerce.Storage.ButtonSnapshot> buttons =
                BVSoftware.Commerce.Storage.DiskStorage.ListButtonsForTheme(BVApp.CurrentStore.Id, themeid);
            foreach (BVSoftware.Commerce.Storage.ButtonSnapshot snapshot in buttons)
            {
                sb.Append("<li id=\"" + snapshot.FileName + "\">");
                sb.Append("<a href=\"#\" title=\"" + snapshot.FileName + "\" class=\"deleteitem\"><img src=\"/images/system/trashcan.png\" alt=\"Delete Button\" /></a>");
                sb.Append("<span class=\"preview\"><img src=\"" + snapshot.Url(true) + "?uid=" + System.Guid.NewGuid().ToString() + "\" alt=\"" + snapshot.FileName + "\" /></span>");
                sb.Append("&nbsp;&nbsp;<span class=\"details\">" + snapshot.FileName + "</span><div class=\"clear\"></div>");
                sb.Append("</li>");
            }
            sb.Append("</ul>");

            this.litMain.Text = sb.ToString();
        }

        protected void btnUpload_Click(object sender, ImageClickEventArgs e)
        {
            string themeId = Request.QueryString["id"];
            if (this.fileupload1.HasFile)
            {
                BVSoftware.Commerce.Storage.DiskStorage.UploadThemeButton(BVApp.CurrentStore.Id, themeId, this.fileupload1.PostedFile);
                LoadTheInfo(themeId);
            }
        }
    }
}