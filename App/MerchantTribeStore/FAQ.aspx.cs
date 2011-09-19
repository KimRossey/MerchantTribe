using System;
using System.Web.UI;
using MerchantTribe.Commerce.Content;

namespace BVCommerce
{

    partial class FAQ : BaseStorePage
    {

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.ManualBreadCrumbTrail1.ClearTrail();
            this.ManualBreadCrumbTrail1.AddLink(SiteTerms.GetTerm(SiteTermIds.Home), "~");
            this.ManualBreadCrumbTrail1.AddLink(SiteTerms.GetTerm(SiteTermIds.CustomerService), "~/ContactUs.aspx");
            this.ManualBreadCrumbTrail1.AddNonLink(SiteTerms.GetTerm(SiteTermIds.FAQ));
            this.Title = SiteTerms.GetTerm(SiteTermIds.FAQ);
            if (!Page.IsPostBack)
            {
                this.TitleLabel.Text = SiteTerms.GetTerm(SiteTermIds.FAQ);
                LoadPolicies();
            }
        }

        void LoadPolicies()
        {
            try
            {
                Policy p = BVApp.ContentServices.Policies.FindOrCreateByType(PolicyType.Faq);
                if (p != null)
                {
                    dlPolicy.DataSource = p.Blocks;
                    dlPolicy.DataBind();
                    dlQuestions.DataSource = p.Blocks;
                    dlQuestions.DataBind();
                }
            }

            catch (Exception Ex)
            {
                msg.Visible = true;
                msg.ShowException(Ex);
            }
        }

    }
}