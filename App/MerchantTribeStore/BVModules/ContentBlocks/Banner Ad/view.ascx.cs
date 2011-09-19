using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.ObjectModel;
using MerchantTribe.Commerce.Content;
using System.Text;

namespace BVCommerce
{

    public partial class BVModules_ContentBlocks_Banner_Ad_view : BVModule
    {
        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            LoadAd();
        }

        private void LoadAd()
        {
            string imageurl = string.Empty;
            string alttext = string.Empty;
            string cssid = string.Empty;
            string cssclass = string.Empty;
            string linkurl = string.Empty;

            ContentBlock b = MyPage.MTApp.ContentServices.Columns.FindBlock(this.BlockId);
            if (b != null)
            {
                imageurl = b.BaseSettings.GetSettingOrEmpty("imageurl");
                alttext = b.BaseSettings.GetSettingOrEmpty("alttext");
                cssid = b.BaseSettings.GetSettingOrEmpty("cssid");
                cssclass = b.BaseSettings.GetSettingOrEmpty("cssclass");
                linkurl = b.BaseSettings.GetSettingOrEmpty("linkurl");

            }

            RenderAd(imageurl, alttext, cssid, cssclass, linkurl);
        }

        private void RenderAd(string imageUrl, string altText, string cssId, string cssClass, string linkUrl)
        {
            StringBuilder sb = new StringBuilder();

            // Open Tag
            if (linkUrl.Trim().Length > 0)
            {
                sb.Append("<a href=\"" + linkUrl + "\" title=\"" + altText + "\" ");
            }
            else
            {
                sb.Append("<span ");
            }

            // Css Id
            if (cssId.Trim().Length > 0)
            {
                sb.Append(" id=\"" + cssId + "\" ");
            }

            // Css Class
            if (cssClass.Trim().Length > 0)
            {
                sb.Append(" class=\"" + cssClass + "\" ");
            }
            sb.Append(">");

            // Image
            sb.Append("<img src=\"" + MerchantTribe.Commerce.Utilities.TagReplacer.ReplaceContentTags(imageUrl,
                                                                                                   MyPage.MTApp,
                                                                                                   "",
                                                                                                   Request.IsSecureConnection)
                                    + "\" alt=\"" + altText + "\" />");

            // Closing Tag
            if (linkUrl.Trim().Length > 0)
            {
                sb.Append("</a>");
            }
            else
            {
                sb.Append("</span>");
            }

            this.litOutput.Text = sb.ToString();
        }

    }
}