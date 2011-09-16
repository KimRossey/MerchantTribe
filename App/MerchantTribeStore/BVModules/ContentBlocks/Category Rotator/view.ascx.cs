using System.Collections.ObjectModel;
using System.Web.UI;
using BVSoftware.Commerce.Catalog;
using BVSoftware.Commerce.Content;
using BVSoftware.Commerce.Utilities;
using System.Collections.Generic;

namespace BVCommerce
{

    partial class BVModules_ContentBlocks_Category_Rotator_view : BVModule
    {        
        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            bool showInOrder = false;

            ContentBlock b = MyPage.BVApp.ContentServices.Columns.FindBlock(this.BlockId);
            if (b != null)
            {
                showInOrder = b.BaseSettings.GetBoolSetting("ShowInOrder");
            }

            int nextIndex;
            if (Session[this.BlockId + "NextImageIndex"] != null)
            {
                nextIndex = (int)Session[this.BlockId + "NextImageIndex"];
            }
            else
            {
                nextIndex = 0;
            }

            List<ContentBlockSettingListItem> settings = b.Lists.FindList("Categories");

            if (settings.Count != 0)
            {
                if (settings.Count > nextIndex)
                {
                    LoadCategory(settings[nextIndex].Setting1);
                }
                else if (nextIndex >= settings.Count)
                {
                    if (showInOrder)
                    {
                        nextIndex = 0;
                    }
                    else
                    {
                        nextIndex = MerchantTribe.Web.RandomNumbers.RandomInteger(settings.Count - 1, 0);
                    }
                    LoadCategory(settings[nextIndex].Setting1);
                }

                if (showInOrder)
                {
                    nextIndex += 1;
                }
                else
                {
                    nextIndex = MerchantTribe.Web.RandomNumbers.RandomInteger(settings.Count - 1, 0);
                }
                Session[this.BlockId + "NextImageIndex"] = nextIndex;
            }
        }

        private void LoadCategory(string categoryId)
        {
            Category c = MyPage.BVApp.CatalogServices.Categories.Find(categoryId);
            if (c != null)
            {
                if (c.Bvin != string.Empty)
                {
                    string destination = UrlRewriter.BuildUrlForCategory(new CategorySnapshot(c), MyPage.BVApp.CurrentRequestContext.RoutingContext);

                    if (c.ImageUrl.StartsWith("~") | c.ImageUrl.StartsWith("http://"))
                    {
                        this.CategoryIconLink.ImageUrl = ImageHelper.SafeImage(Page.ResolveUrl(c.ImageUrl));
                    }
                    else
                    {
                        this.CategoryIconLink.ImageUrl = ImageHelper.SafeImage(Page.ResolveUrl("~/" + c.ImageUrl));
                    }

                    this.CategoryIconLink.NavigateUrl = destination;
                    this.CategoryIconLink.ToolTip = c.MetaTitle;

                    this.CategoryLink.Text = c.Name;
                    this.CategoryLink.NavigateUrl = destination;
                    this.CategoryLink.ToolTip = c.MetaTitle;

                    if (c.SourceType == CategorySourceType.CustomLink)
                    {
                        if (c.CustomPageOpenInNewWindow == true)
                        {
                            this.CategoryLink.Target = "_blank";
                            this.CategoryIconLink.Target = "_blank";
                        }
                    }

                }
            }
        }
    }
}