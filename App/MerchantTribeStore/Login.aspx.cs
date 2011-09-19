using System;
using System.Web.UI;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Content;

namespace BVCommerce
{

    partial class login2 : BaseStorePage
    {

        public override bool RequiresSSL
        {
            get { return true; }
        }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.LoginControl1.LoginCompleted += this.LoginCompleted;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            LoginControl1.LoginCompleted += LoginCompleted;
            NewUserControl1.LoginCompleted += LoginCompleted;

            this.ManualBreadCrumbTrail1.ClearTrail();
            this.ManualBreadCrumbTrail1.AddLink(SiteTerms.GetTerm(SiteTermIds.Home), "~");
            this.ManualBreadCrumbTrail1.AddNonLink(SiteTerms.GetTerm(SiteTermIds.Login));
            if (!Page.IsPostBack)
            {

                if (MTApp.CurrentStore.Settings.IsPrivateStore)
                {
                    this.pnlNewUser.Visible = false;
                    this.ContactUsHyperLink.Text = SiteTerms.GetTerm(SiteTermIds.PrivateStoreNewUser);
                    this.ContactUsHyperLink.Visible = true;
                }
                else
                {
                    this.pnlNewUser.Visible = true;
                    this.ContactUsHyperLink.Visible = false;
                }

                TitleLabel.Text = SiteTerms.GetTerm(SiteTermIds.Login);
                Page.Title = SiteTerms.GetTerm(SiteTermIds.Login);

                if (Request.Params["ReturnTo"] != null)
                {
                    //then we set the ReturnURL after they login
                    ViewState["ReturnTo"] = "Checkout";
                }
                else if (Request.Params["ReturnURL"] != null)
                {
                    ViewState["ReturnURL"] = Request.Params["ReturnURL"];
                }

            }
        }

        private void RedirectOnComplete(string userId)
        {
            if ((ViewState["ReturnTo"] != null) && (userId != string.Empty))
            {
                if (((string)ViewState["ReturnTo"]).ToLower() == "checkout")
                {
                    Response.Redirect("~/checkout");
                }
            }
            else if ((ViewState["ReturnURL"] != null) && (userId != string.Empty))
            {
                Response.Redirect((string)ViewState["ReturnURL"]);
            }
            else
            {

                Response.Redirect("~/account/");

            }
        }

        public void LoginCompleted(object sender, MerchantTribe.Commerce.Controls.LoginCompleteEventArgs args)
        {
            RedirectOnComplete(args.UserId);
        }
    }
}