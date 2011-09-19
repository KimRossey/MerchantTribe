using System;
using System.Web.UI;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Content;
using MerchantTribe.Commerce.Membership;
using MerchantTribe.Commerce.Utilities;

namespace BVCommerce
{

    partial class EmailFriend : BaseStorePage
    {

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {

                btnSend.ImageUrl = this.MTApp.ThemeManager().ButtonUrl("Submit", Request.IsSecureConnection);

                inMessage.Text = "<a href=\"" + Request.Params["page"] + "\">" + Request.Params["page"] + "</a>";

                if (SessionManager.IsUserAuthenticated(this.MTApp) == true)
                {

                    CustomerAccount u = MTApp.CurrentCustomer;

                    this.FromEmailField.Text = u.Email;
                }

                this.pnlMain.Visible = true;
                this.pnlRegister.Visible = false;

                //Me.valEmail.Text = ImageHelper.GetErrorIconTag
                //Me.valEmail2.Text = ImageHelper.GetErrorIconTag
                //Me.Requiredfieldvalidator1.Text = ImageHelper.GetErrorIconTag
                //Me.Regularexpressionvalidator1.Text = ImageHelper.GetErrorIconTag
            }
        }

        protected void btnSend_OnClick(object Sender, ImageClickEventArgs E)
        {
            lblErrorMessage.Visible = false;
            lblErrorMessage.Text = "";
            lblResults.Text = "";
            string f = string.Empty;

            Product p;
            p = MTApp.CatalogServices.Products.Find(Request.QueryString["productID"]);

            HtmlTemplate t = MTApp.ContentServices.GetHtmlTemplateOrDefault(HtmlTemplateType.EmailFriend);

            if (t != null)
            {

                System.Net.Mail.MailMessage m;
                t = t.ReplaceTagsInTemplate(MTApp, p);
                t.From = this.FromEmailField.Text.Trim();
                m = t.ConvertToMailMessage(this.toEmailField.Text.Trim());

                if (MailServices.SendMail(m) == false)
                {
                    lblErrorMessage.Text = "Error while sending mail!";
                    lblErrorMessage.Visible = true;
                }
                else
                {
                    lblResults.Text = "Thank you.  Your message has been sent.";
                }

            }

        }

    }
}