using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BVSoftware.Commerce.Content;
using BVSoftware.Commerce.Utilities;

namespace BVCommerce
{

    partial class BVModules_Controls_Pager : System.Web.UI.UserControl
    {

        public delegate void PageChangeDelegate();
        public event PageChangeDelegate PageChange;

        private int _pages = 0;
        private int _rowCount = 0;
        private int _itemsPerPage = 10;

        private int _page = int.MinValue;

        public int CurrentRow
        {
            get
            {
                int val = ((CurrentPage - 1) * ItemsPerPage);
                return val;
            }
        }

        public int CurrentPage
        {
            get
            {
                if (_page != int.MinValue)
                {
                    return _page;
                }
                else
                {
                    string page = Request.QueryString["page"];
                    if (page != null)
                    {
                        int currPage = int.Parse(page);
                        return int.Parse(page);
                    }
                    else
                    {
                        return 1;
                    }
                }
            }
            set { _page = value; }
        }

        public int RowCount
        {
            get { return _rowCount; }
            set
            {
                _rowCount = value;
                this.InitializeButtons();
            }
        }

        public int ItemsPerPage
        {
            get { return _itemsPerPage; }
            set { _itemsPerPage = value; }
        }

        public int GetPageCount
        {
            get
            {
                if (_pages == 0)
                {
                    _pages = this.RowCount / this.ItemsPerPage;
                    if (this.RowCount % this.ItemsPerPage > 0)
                    {
                        _pages += 1;
                    }
                }
                return _pages;
            }
        }

        private string _baseUrl = string.Empty;
        public string BaseUrl
        {
            get { return _baseUrl; }
            set { _baseUrl = value; }
        }

        public void InitializeButtons()
        {
            if (this.GetPageCount <= 1)
            {
                this.Visible = false;
            }
            else
            {
                this.Visible = true;
            }

            string currentUrl = GetCurrentUrl();

            if ((currentUrl == string.Empty))
            {
                return;
            }

            if ((currentUrl.Contains("{page}")))
            {

            }


            if (this.CurrentPage == 1)
            {
                FirstListItem.InnerText = SiteTerms.GetTerm(SiteTermIds.First);
                PreviousListItem.InnerText = SiteTerms.GetTerm(SiteTermIds.Previous);
                this.FirstListItem.Attributes.Add("class", "disabled");
                this.PreviousListItem.Attributes.Add("class", "disabled");
            }
            else
            {
                HyperLink FirstHyperLink = new HyperLink();
                FirstHyperLink.Text = SiteTerms.GetTerm(SiteTermIds.First);
                FirstHyperLink.NavigateUrl = currentUrl.Replace("{page}", "1");
                FirstListItem.Controls.Clear();
                FirstListItem.Controls.Add(FirstHyperLink);

                HyperLink PreviousHyperLink = new HyperLink();
                PreviousHyperLink.Text = SiteTerms.GetTerm(SiteTermIds.Previous);
                PreviousHyperLink.NavigateUrl = currentUrl.Replace("{page}", (this.CurrentPage - 1).ToString());
                PreviousListItem.Controls.Clear();
                PreviousListItem.Controls.Add(PreviousHyperLink);

                this.FirstListItem.Attributes.Remove("class");
                this.PreviousListItem.Attributes.Remove("class");
            }

            if (this.CurrentPage == this.GetPageCount)
            {
                LastListItem.InnerText = SiteTerms.GetTerm(SiteTermIds.Last);
                NextListItem.InnerText = SiteTerms.GetTerm(SiteTermIds.Next);
                this.LastListItem.Attributes.Add("class", "disabled");
                this.NextListItem.Attributes.Add("class", "disabled");
            }
            else
            {
                HyperLink LastHyperLink = new HyperLink();
                LastHyperLink.Text = SiteTerms.GetTerm(SiteTermIds.Last);
                LastHyperLink.NavigateUrl = currentUrl.Replace("{page}", (this.GetPageCount).ToString());
                LastListItem.Controls.Clear();
                LastListItem.Controls.Add(LastHyperLink);

                HyperLink NextHyperLink = new HyperLink();
                NextHyperLink.Text = SiteTerms.GetTerm(SiteTermIds.Next);
                NextHyperLink.NavigateUrl = currentUrl.Replace("{page}", (this.CurrentPage + 1).ToString());
                NextListItem.Controls.Clear();
                NextListItem.Controls.Add(NextHyperLink);

                this.LastListItem.Attributes.Remove("class");
                this.NextListItem.Attributes.Remove("class");
            }

            int pageSet = ((this.CurrentPage - 1) / 10);
            int startPageNumber = (pageSet * 10) + 1;

            PagesPlaceHolder.Controls.Clear();
            for (int i = 1; i <= 11; i++)
            {
                if ((startPageNumber + (i - 1)) <= this.GetPageCount)
                {
                    HtmlGenericControl li = new HtmlGenericControl("li");
                    PagesPlaceHolder.Controls.Add(li);
                    if (i <= 10)
                    {
                        HyperLink lb = new HyperLink();
                        lb.ID = "HyperLink" + i.ToString();
                        lb.Text = (startPageNumber + (i - 1)).ToString();

                        string url = currentUrl.Replace("{page}", (startPageNumber + (i - 1)).ToString());
                        lb.NavigateUrl = url;
                        li.Controls.Add(lb);

                        if (lb.Text == this.CurrentPage.ToString())
                        {
                            lb.Enabled = false;
                            li.Attributes.Add("class", "current");
                        }
                        else
                        {
                            lb.Enabled = true;
                            li.Attributes.Remove("class");
                        }
                    }
                    else
                    {
                        HyperLink lb = new HyperLink();
                        lb.ID = "HyperLink" + i.ToString();
                        lb.Text = "...";
                        string url = currentUrl;
                        lb.NavigateUrl = url.Replace("{page}", (startPageNumber + (i - 1)).ToString());
                        li.Controls.Add(lb);
                    }
                }
            }

        }

        private string GetCurrentUrl()
        {

            string result = string.Empty;

            if ((_baseUrl != string.Empty))
            {
                result = _baseUrl;
            }

            // Only get base url if none was supplied
            if ((result == string.Empty))
            {
                if (this.Page is BaseStoreCategoryPage)
                {
                    BaseStoreCategoryPage CategoryPage = (BaseStoreCategoryPage)this.Page;
                    if (CategoryPage.LocalCategory != null)
                    {
                        result = UrlRewriter.BuildUrlForCategory(new BVSoftware.Commerce.Catalog.CategorySnapshot(CategoryPage.LocalCategory), CategoryPage.Request.RequestContext);
                    }
                }
                else if (this.Page is BaseSearchPage)
                {
                    result = Page.ResolveUrl("~/search");
                }

                if (result.Contains("?"))
                {
                    result += "&page={page}";
                }
                else
                {
                    result += "?page={page}";
                }
            }

            return result;
        }

    }
}