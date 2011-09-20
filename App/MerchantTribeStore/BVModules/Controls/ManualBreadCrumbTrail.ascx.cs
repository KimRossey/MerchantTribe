using System.Web.UI;
using MerchantTribe.Commerce.Content;

namespace MerchantTribeStore
{

    partial class BVModules_Controls_ManualBreadCrumbTrail : System.Web.UI.UserControl
    {

        private string _Spacer = SiteTerms.GetTerm(SiteTermIds.BreadcrumbTrailSeparator);
        private string _Trail = string.Empty;

        public string Spacer
        {
            get { return _Spacer; }
            set { _Spacer = value; }
        }
        public string Trail
        {
            get { return _Trail; }
            set { _Trail = value; }
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            LoadTrail();
        }

        public void LoadTrail()
        {

            this.TrailPlaceholder.Controls.Clear();

            string[] links = _Trail.Split(',');
            if (links.Length > 0)
            {
                for (int i = 0; i <= links.Length - 1; i++)
                {

                    string[] data = links[i].Split('|');
                    if (data.Length > 1)
                    {
                        this.TrailPlaceholder.Controls.Add(new LiteralControl("<a href=\"" + Page.ResolveUrl(data[1]) + "\">" + data[0] + "</a>"));
                    }
                    else
                    {
                        if (data.Length > 0)
                        {
                            this.TrailPlaceholder.Controls.Add(new LiteralControl(data[0]));
                        }
                    }

                    if (i != links.Length - 1)
                    {
                        this.TrailPlaceholder.Controls.Add(new LiteralControl("<span class=\"spacer\">" + _Spacer + "</span>"));
                    }
                }

            }

        }

        public void ClearTrail()
        {
            _Trail = string.Empty;
        }

        public void AddLink(string name, string navigateurl)
        {
            if (_Trail.Length > 0)
            {
                _Trail += ",";
            }
            _Trail += name.Replace(",", "");
            _Trail += "|";
            _Trail += navigateurl.Replace(",", "");
        }

        public void AddNonLink(string name)
        {
            if (_Trail.Length > 0)
            {
                _Trail += ",";
            }
            _Trail += name.Replace(",", "");
        }

    }
}