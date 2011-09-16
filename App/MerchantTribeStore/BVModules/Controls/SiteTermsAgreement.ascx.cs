using System.Web.UI;
using BVSoftware.Commerce;
using BVSoftware.Commerce.Content;

namespace BVCommerce
{

    partial class BVModules_Controls_SiteTermsAgreement : BVSoftware.Commerce.Content.BVModule
    {

        public bool IsValid
        {
            get
            {
                if (MyPage.BVApp.CurrentStore.Settings.ForceTermsAgreement)
                {
                    return AgreeToTermsCheckBox.Checked;
                }
                else
                {
                    return true;
                }
            }
            set { AgreeToTermsCheckBox.Checked = value; }
        }

        protected override void OnInit(System.EventArgs e)
        {
            base.OnInit(e);

            if (MyPage.BVApp.CurrentStore.Settings.ForceTermsAgreement)
            {
                this.Visible = true;
                AgreeLiteral.Text = SiteTerms.GetTerm(SiteTermIds.TermsAndConditionsAgreement);
                ViewSiteTermsLiteral.Text = SiteTerms.GetTerm(SiteTermIds.TermsAndConditions);
                ShippingHyperLink.Attributes.Add("onclick", "javascript:window.open('" + Page.ResolveUrl("~/TermsPopUp.aspx") + "','Policy','width=400, height=500, menubar=no, scrollbars=yes, resizable=yes, status=no, toolbar=no')");
                ShippingHyperLink.NavigateUrl = "javascript:void(0);";
            }
            else
            {
                this.Visible = false;
            }
        }
    }
}