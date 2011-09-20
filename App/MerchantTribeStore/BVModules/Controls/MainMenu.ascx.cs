using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.HtmlControls;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Content;
using MerchantTribe.Commerce.Utilities;

namespace MerchantTribeStore
{

    partial class BVModules_Controls_MainMenu : BVUserControl
    {

        private int _LinksPerRow = 7;
        private int _MaximumLinks = 0;
        private int _tabIndex = -1;


        private int _tempTabIndex = 0;
        public int TabIndex
        {
            get { return _tabIndex; }
            set { _tabIndex = value; }
        }

        public int LinksPerRow
        {
            get { return _LinksPerRow; }
            set { _LinksPerRow = value; }
        }
        public int MaximumLinks
        {
            get { return _MaximumLinks; }
            set { _MaximumLinks = value; }
        }

        private bool _DisplayActiveTab = false;

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            _DisplayActiveTab = ((BaseStorePage)this.Page).DisplaysActiveCategoryTab;
            LoadMenu();
        }

        private void LoadMenu()
        {
            this.litMain.Text = RenderMenu(LinksPerRow, MaximumLinks, TabIndex);
        }

        private string RenderMenu(int linksPerRow, int maxLinks, int tabIndex)
        {
            // Find Categories to Display in Menu
            List<CategorySnapshot> categories = MyPage.MTApp.CatalogServices.Categories.FindForMainMenu();

            // Limit number of links
            int stopCount = categories.Count - 1;
            if ((_MaximumLinks > 0) && ((_MaximumLinks - 1) < stopCount))
            {
                stopCount = (_MaximumLinks - 1);
            }

            StringBuilder sb = new StringBuilder();

            // Open List
            if (categories.Count > 0)
            {
                sb.Append("<ul>");
            }

            _tempTabIndex = this.TabIndex;
            // Build each Row
            for (int i = 0; i <= (stopCount); i++)
            {
                sb.Append(BuildLink(categories[i]));
                // Move to Next Row if Not Last Item
                if ((((i + 1) % _LinksPerRow) == 0) && ((i) < stopCount))
                {
                    sb.Append("</ul><ul>");
                }
            }

            // Close List
            if (categories.Count > 0)
            {
                sb.Append("</ul>");
            }

            return sb.ToString();
        }

        private string BuildLink(CategorySnapshot c)
        {
            string result = string.Empty;

            StringBuilder sbLink = new StringBuilder();

            sbLink.Append("<li");
            if ((c.Bvin == SessionManager.CategoryLastId) && _DisplayActiveTab)
            {
                sbLink.Append(" class=\"activemainmenuitem\" >");
            }
            else
            {
                sbLink.Append(">");
            }

            sbLink.Append("<a href=\"");
            sbLink.Append(UrlRewriter.BuildUrlForCategory(c, this.Page.Request.RequestContext));
            if (c.CustomPageOpenInNewWindow)
            {
                sbLink.Append("\" target=\"_blank\"");
            }
            else
            {
                sbLink.Append("\"");
            }
            if (_tempTabIndex != -1)
            {
                sbLink.Append(" TabIndex=\"" + _tempTabIndex.ToString() + "\" ");
                _tempTabIndex += 1;
            }
            sbLink.Append(" class=\"actuator\" title=\"" + c.MetaTitle + "\">");
            sbLink.Append(c.Name);
            sbLink.Append("</a></li>");

            result = sbLink.ToString();
            return result;
        }
        protected void BuildListItem(HtmlGenericControl item, CategorySnapshot category)
        {
            item.InnerText = category.Name;

            if ((category.Bvin == SessionManager.CategoryLastId) && _DisplayActiveTab)
            {
                item.Attributes["class"] = "activemainmenuitem";
            }
            else
            {
                //item.Attributes("class") = "menuitem"
            }

            StringBuilder sbLink = new StringBuilder();
            sbLink.Append("<a href=\"");
            sbLink.Append(UrlRewriter.BuildUrlForCategory(category, this.Page.Request.RequestContext));
            if (category.CustomPageOpenInNewWindow)
            {
                sbLink.Append("\" target=\"_blank");
            }
            sbLink.Append("\" class=\"actuator\">");
            sbLink.Append(category.Name);
            sbLink.Append("</a>");
            item.InnerHtml = sbLink.ToString();
        }

    }
}