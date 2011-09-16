using System.Web.UI.WebControls;
using BVSoftware.Commerce;
using BVSoftware.Commerce.Catalog;
using BVSoftware.Commerce.Content;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace BVCommerce
{

    partial class BVAdmin_Controls_CategoryBreadCrumbTrail : BVSoftware.Commerce.Content.BVUserControl
    {

        private string _Spacer = "&nbsp;::&nbsp;";
        private bool _DisplayLinks = false;

        public string Spacer
        {
            get { return _Spacer; }
            set { _Spacer = value; }
        }

        public bool DisplayLinks
        {
            get { return _DisplayLinks; }
            set { _DisplayLinks = value; }
        }

        public void LoadTrail(string categoryId)
        {

            this.TrailPlaceholder.Controls.Clear();

            List<CategorySnapshot> trail = new List<CategorySnapshot>();
            trail = Category.BuildTrailToRoot(categoryId, MyPage.BVApp.CurrentRequestContext);

            if (_DisplayLinks)
            {
                HyperLink m = new HyperLink();
                m.CssClass = "root";
                m.ToolTip = SiteTerms.GetTerm(SiteTermIds.Home);
                m.Text = SiteTerms.GetTerm(SiteTermIds.Home);
                m.NavigateUrl = this.Page.ResolveUrl("~/");
                m.EnableViewState = false;
                this.TrailPlaceholder.Controls.Add(m);
            }
            else
            {
                this.TrailPlaceholder.Controls.Add(new System.Web.UI.LiteralControl("<span class=\"current\">Home</span>"));
            }

            this.TrailPlaceholder.Controls.Add(new System.Web.UI.LiteralControl("<span class=\"spacer\">" + _Spacer + "</span>"));
            if (trail != null)
            {
                // Walk list backwards
                for (int i = trail.Count - 1; i >= 0; i += -1)
                {
                    if (i != trail.Count - 1)
                    {
                        this.TrailPlaceholder.Controls.Add(new System.Web.UI.LiteralControl("<span class=\"spacer\">" + _Spacer + "</span>"));
                    }
                    if (i != 0)
                    {
                        if (_DisplayLinks)
                        {
                            AddCategoryLink(trail[i]);
                        }
                        else
                        {
                            AddCategoryName(trail[i]);
                        }
                    }

                    else
                    {
                        AddCategoryName(trail[i]);
                    }
                }
            }

        }

        private void AddCategoryLink(CategorySnapshot c)
        {
            HyperLink m = new HyperLink();
            m.ToolTip = c.MetaTitle;
            m.Text = c.Name;

            m.NavigateUrl = BVSoftware.Commerce.Utilities.UrlRewriter.BuildUrlForCategory(c, MyPage.BVApp.CurrentRequestContext.RoutingContext);
            if (c.SourceType == CategorySourceType.CustomLink)
            {
                if (c.CustomPageOpenInNewWindow == true)
                {
                    m.Target = "_blank";
                }
            }

            m.EnableViewState = false;
            this.TrailPlaceholder.Controls.Add(m);
        }

        private void AddCategoryName(CategorySnapshot c)
        {
            this.TrailPlaceholder.Controls.Add(new System.Web.UI.LiteralControl("<span class=\"current\">" + c.Name + "</span>"));
        }


    }
}