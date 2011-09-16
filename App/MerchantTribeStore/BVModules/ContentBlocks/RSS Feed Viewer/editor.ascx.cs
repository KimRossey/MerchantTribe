using System.Web.UI;
using BVSoftware.Commerce.Content;

namespace BVCommerce
{

    partial class BVModules_ContentBlocks_RSS_Feed_Viewer_editor : BVModule
    {

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {
                LoadData();
            }
        }

        private void LoadData()
        {
            ContentBlock b = MyPage.BVApp.ContentServices.Columns.FindBlock(this.BlockId);
            if (b != null)
            {
                this.FeedField.Text = b.BaseSettings.GetSettingOrEmpty("FeedUrl");
                this.chkShowDescription.Checked = b.BaseSettings.GetBoolSetting("ShowDescription");
                this.chkShowTitle.Checked = b.BaseSettings.GetBoolSetting("ShowTitle");
                this.MaxItemsField.Text = b.BaseSettings.GetBoolSetting("MaxItems").ToString();
            }
        }

        private void SaveData()
        {
            ContentBlock b = MyPage.BVApp.ContentServices.Columns.FindBlock(this.BlockId);
            if (b != null)
            {
                b.BaseSettings.AddOrUpdate("FeedUrl", this.FeedField.Text.Trim());
                b.BaseSettings.SetBoolSetting("ShowDescription", this.chkShowDescription.Checked);
                b.BaseSettings.SetBoolSetting("ShowTitle", this.chkShowTitle.Checked);
                b.BaseSettings.SetIntegerSetting("MaxItems", int.Parse(this.MaxItemsField.Text.Trim()));
                MyPage.BVApp.ContentServices.Columns.UpdateBlock(b);
            }
        }

        protected void btnCancel_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            this.NotifyFinishedEditing();
        }

        protected void btnSave_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            SaveData();
            this.NotifyFinishedEditing();
        }

    }
}