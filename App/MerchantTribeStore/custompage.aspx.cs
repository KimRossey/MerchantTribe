using System.Collections.ObjectModel;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Utilities;

namespace MerchantTribeStore
{

    public partial class custompage : BaseStoreCategoryPage
    {
        protected override void OnPreInit(System.EventArgs e)
        {
            base.OnPreInit(e);
        }

        protected override void OnInit(System.EventArgs e)
        {
            base.OnInit(e);
            this.AddBodyClass("store-custom-page");
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            if (LocalCategory != null)
            {
                if (LocalCategory.SourceType != CategorySourceType.CustomPage)
                {
                    Response.Redirect(LocalCategory.RewriteUrl);
                }
                PopulateCategoryInfo();
                RecordCategoryView();
            }
        }

        private void RecordCategoryView()
        {
            SessionManager.CategoryLastId = LocalCategory.Bvin;
        }

        public void PopulateCategoryInfo()
        {

            if (LocalCategory.CustomPageLayout == CustomPageLayoutType.WithSideBar)
            {
                this.ContentColumnControl1.Visible = true;
                this.litPreLeft.Text = "<div id=\"categoryleft\">";
                this.litPostLeft.Text = "</div><div id=\"categorymain\">";
            }
            else
            {
                this.ContentColumnControl1.Visible = false;
                this.litPreLeft.Text = "";
                this.litPostLeft.Text = "<div class=\"onecolumnmain\">";
            }

            // Page Title
            if (LocalCategory.MetaTitle.Trim().Length > 0)
            {
                this.Title = LocalCategory.MetaTitle;
            }
            else
            {
                this.Title = LocalCategory.Name;
            }

            // Meta Keywords
            if (LocalCategory.MetaKeywords.Trim().Length > 0)
            {
                Page.MetaKeywords = LocalCategory.MetaKeywords;
            }

            // Meta Description
            if (LocalCategory.MetaDescription.Trim().Length > 0)
            {
                Page.MetaDescription = LocalCategory.MetaDescription;
            }

            this.litMain.Text = MerchantTribe.Commerce.Utilities.TagReplacer.ReplaceContentTags(LocalCategory.Description,
                                                                                            this.MTApp,
                                                                                            "", Request.IsSecureConnection);

        }

    }
}