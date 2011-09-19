using MerchantTribe.Commerce.Content;

namespace BVCommerce
{

    partial class BVModules_ContentBlocks_Html_view : BVModule
    {

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            LoadText();
        }

        private void LoadText()
        {
            string result = string.Empty;
            ContentBlock b = MyPage.BVApp.ContentServices.Columns.FindBlock(this.BlockId);
            if (b != null)
            {
                result = b.BaseSettings.GetSettingOrEmpty("HtmlData");
            }

            result = MerchantTribe.Commerce.Utilities.TagReplacer.ReplaceContentTags(result,
                                                                                    MyPage.BVApp,
                                                                                    "",
                                                                                    Request.IsSecureConnection);
            this.HtmlContent.Text = result;
        }

    }
}