using System.Web.UI;
using BVSoftware.Commerce.Content;

namespace BVCommerce
{

    partial class BVModules_Controls_Search : BVSoftware.Commerce.Content.BVUserControl
    {

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {
                SetDefaultControlValues();
            }
        }

        private void SetDefaultControlValues()
        {
            lblTitle.Text = SiteTerms.GetTerm(SiteTermIds.Search);
            btnSearch.ImageUrl = MyPage.BVApp.ThemeManager().ButtonUrl("gosearch", Request.IsSecureConnection);
            btnSearch.AlternateText = "Submit Form";
        }

        protected void btnSearch_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            Response.Redirect("~/search.aspx?q=" + Server.UrlEncode(this.KeywordField.Text.Trim()) + "&p=1");
        }

    }
}