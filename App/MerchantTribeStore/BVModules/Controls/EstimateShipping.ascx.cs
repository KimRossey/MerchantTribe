using System.Web.UI;

namespace MerchantTribeStore
{

    partial class BVModules_Controls_EstimateShipping : MerchantTribe.Commerce.Content.BVUserControl
    {

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {
                this.imgGetRates.ImageUrl = MyPage.MTApp.ThemeManager().ButtonUrl("estimateshipping", Request.IsSecureConnection);
            }

            int w = 400;
            int h = 500;

            this.GetRatesLink.Style.Add("CURSOR", "pointer");
            this.GetRatesLink.Attributes.Add("onclick", "JavaScript:window.open('" + Page.ResolveUrl("~/EstimateShipping") + "','Images','width=" + w + ", height=" + h + ", menubar=no, scrollbars=yes, resizable=yes, status=no, toolbar=no')");

        }

    }
}