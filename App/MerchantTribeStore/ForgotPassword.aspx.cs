using System.Web;
using System.Web.UI;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Content;
using MerchantTribe.Commerce.Membership;
using MerchantTribe.Commerce.Utilities;
using System.Collections.Generic;

namespace BVCommerce
{

    partial class ForgotPassword : BaseStorePage
    {

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            this.ManualBreadCrumbTrail1.ClearTrail();
            this.ManualBreadCrumbTrail1.AddLink(SiteTerms.GetTerm(SiteTermIds.Home), "~");
            this.ManualBreadCrumbTrail1.AddLink(SiteTerms.GetTerm(SiteTermIds.CustomerService), "~/ContactUs.aspx");
            this.ManualBreadCrumbTrail1.AddNonLink(SiteTerms.GetTerm(SiteTermIds.ForgotPassword));


            if (!Page.IsPostBack)
            {

                Page.Title = SiteTerms.GetTerm(SiteTermIds.ForgotPassword);

                string email = (string)RouteData.Values["email"];
                if (email != null && email != string.Empty)
                {
                    this.inUsername.Text = email;
                }

                btnSend.ImageUrl = this.MTApp.ThemeManager().ButtonUrl("Submit", Request.IsSecureConnection);
                this.lblUsername.Text = "Email:";
                val2Username.ErrorMessage = "Please enter am email address";
            }
        }

        protected void btnSend_Click(System.Object sender, System.Web.UI.ImageClickEventArgs e)
        {

            msg.ClearMessage();

            if (Page.IsValid)
            {

                try
                {
                    string userEmail = inUsername.Text.Trim();
                    if (userEmail.StartsWith("@")) userEmail = userEmail.TrimStart('@');
                    if (userEmail.Trim().Length < 5)
                    {
                        msg.ShowWarning("Please enter a valid email address.");
                        return;
                    }

                    CustomerAccount thisUser = MTApp.MembershipServices.Customers.FindByEmail(inUsername.Text);
                    string newPassword = string.Empty;

                    if ((thisUser != null) && (thisUser.Bvin != string.Empty))
                    {                       
                        newPassword = MTApp.MembershipServices.GeneratePasswordForCustomer(WebAppSettings.PasswordMinimumLength + 2);
                        thisUser.Password = thisUser.EncryptPassword(newPassword);
                        if (MTApp.MembershipServices.UpdateCustomer(thisUser))
                        {
                            HtmlTemplate t = MTApp.ContentServices.GetHtmlTemplateOrDefault(HtmlTemplateType.ForgotPassword);                            
                            if (t != null)
                            {
                                System.Net.Mail.MailMessage m;
                                List<IReplaceable> tempList = new List<IReplaceable>();
                                tempList.Add(thisUser);
                                tempList.Add(new Replaceable("[[NewPassword]]", newPassword));

                                t = t.ReplaceTagsInTemplate(MTApp, tempList);

                                m = t.ConvertToMailMessage(thisUser.Email);

                                if (MailServices.SendMail(m) == false)
                                {
                                    msg.ShowError("Error while sending mail!");
                                }

                            }

                            msg.ShowOk("Your new password has been sent by email.");
                        }
                        else
                        {
                            msg.ShowError("An error occurred while trying to update password.");
                        }
                    }
                    else
                    {
                        msg.ShowError(SiteTerms.GetTerm(SiteTermIds.Username) + " not found.");
                    }
                }


                catch (SystemMembershipUserException CreateEx)
                {
                    switch (CreateEx.Status)
                    {
                        case CreateUserStatus.UpdateFailed:
                            msg.ShowError("Update to user account failed.");
                            break;
                        default:
                            msg.ShowError(CreateEx.Message);
                            break;
                    }
                }
            }
            else
            {
                msg.ShowWarning("Please check your " + SiteTerms.GetTerm(SiteTermIds.Username) + ".  No account was found for " + HttpUtility.HtmlEncode(inUsername.Text));
            }

        }

        protected void lnkClose_Click(object sender, System.EventArgs e)
        {
            string destination = GetRouteUrl("customerlogin", new object());
           
            if ((string)Page.RouteData.Values["checkout"] == "1")
            {
                destination = GetRouteUrl("checkout-route", new object());
            }

            if (destination == string.Empty) destination = "~/signin";

            Response.Redirect(destination);
        }

    }
}