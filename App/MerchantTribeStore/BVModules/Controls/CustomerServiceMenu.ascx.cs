using System.Web.UI;
using MerchantTribe.Commerce.Content;

namespace MerchantTribeStore
{

    partial class BVModules_Controls_CustomerServiceMenu : System.Web.UI.UserControl
    {


        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {
                //Me.TitleLabel.Text = Content.SiteTerms.GetTerm("CustomerService")
                //lstCustomerServiceMenu.InnerHtml = Nothing
                //lstCustomerServiceMenu.InnerHtml += "<li><A href=""contact.aspx"">" & Content.SiteTerms.GetTerm("ContactUs") & "</A></li>"
                //lstCustomerServiceMenu.InnerHtml += "<li><A href=""help.aspx"">" & Content.SiteTerms.GetTerm("Help") & "</A></li>"
                //lstCustomerServiceMenu.InnerHtml += "<li><A href=""privacy.aspx"">" & Content.SiteTerms.GetTerm("PrivacyPolicy") & "</A></li>"
                //lstCustomerServiceMenu.InnerHtml += "<li><A href=""terms.aspx"">" & Content.SiteTerms.GetTerm("TermsAndConditions") & "</A></li>"
                //lstCustomerServiceMenu.InnerHtml += "<li><A href=""myaccount_orders.aspx"">" & Content.SiteTerms.GetTerm("OrderHistory") & "</A></li>"
                //If WebAppSettings.AffiliateSignupAllowed Then
                //    'lstCustomerServiceMenu.InnerHtml += "<li><A href=""affiliate_intro.aspx"">" & Content.SiteTerms.GetTerm("AffiliateProgram") & "</A></li>"
                //    lstCustomerServiceMenu.InnerHtml += "<li><A href=""affiliate_intro.aspx"">Affiliate Program</A></li>"
                //End If
                //If WebAppSettings.GiftCertificatesAllowed Then
                //    'lstCustomerServiceMenu.InnerHtml += "<li><A href=""GiftCertificates.aspx"">" & Content.SiteTerms.GetTerm("GiftCertificates") & "</A></li>"
                //    lstCustomerServiceMenu.InnerHtml += "<li><A href=""GiftCertificate.aspx"">Gift Certificates</A></li>"
                //End If
                //If WebAppSettings.ReturnFormAllowed Then
                //    'lstCustomerServiceMenu.InnerHtml += "<li><A href=""ReturnForm.aspx"">" & Content.SiteTerms.GetTerm("ReturnRequests") & "</A></li>"
                //    lstCustomerServiceMenu.InnerHtml += "<li><A href=""ReturnForm.aspx"">Return Requests</A></li>"
                //End If

                this.TitleLabel.Text = SiteTerms.GetTerm(SiteTermIds.CustomerService);
                lstCustomerServiceMenu.InnerHtml = null;
                lstCustomerServiceMenu.InnerHtml += "<li><A href=\"contactus.aspx\">" + SiteTerms.GetTerm(SiteTermIds.ContactUs) + "</A></li>";
                lstCustomerServiceMenu.InnerHtml += "<li><A href=\"help.aspx\">" + SiteTerms.GetTerm(SiteTermIds.Help) + "</A></li>";
                lstCustomerServiceMenu.InnerHtml += "<li><A href=\"privacy.aspx\">" + SiteTerms.GetTerm(SiteTermIds.PrivacyPolicy) + "</A></li>";
                lstCustomerServiceMenu.InnerHtml += "<li><A href=\"terms.aspx\">" + SiteTerms.GetTerm(SiteTermIds.TermsAndConditions) + "</A></li>";
                lstCustomerServiceMenu.InnerHtml += "<li><A href=\"myaccount_orders.aspx\">" + SiteTerms.GetTerm(SiteTermIds.OrderHistory) + "</A></li>";
                //If WebAppSettings.AffiliateSignupAllowed Then
                //    'lstCustomerServiceMenu.InnerHtml += "<li><A href=""affiliate_intro.aspx"">" & Content.SiteTerms.GetTerm("AffiliateProgram") & "</A></li>"
                //    lstCustomerServiceMenu.InnerHtml += "<li><A href=""affiliate_intro.aspx"">Affiliate Program</A></li>"
                //End If
                //If WebAppSettings.GiftCertificatesAllowed Then
                //    'lstCustomerServiceMenu.InnerHtml += "<li><A href=""GiftCertificates.aspx"">" & Content.SiteTerms.GetTerm("GiftCertificates") & "</A></li>"
                //    lstCustomerServiceMenu.InnerHtml += "<li><A href=""GiftCertificate.aspx"">Gift Certificates</A></li>"
                //End If
                //If WebAppSettings.ReturnFormAllowed Then
                //    'lstCustomerServiceMenu.InnerHtml += "<li><A href=""ReturnForm.aspx"">" & Content.SiteTerms.GetTerm("ReturnRequests") & "</A></li>"
                //    lstCustomerServiceMenu.InnerHtml += "<li><A href=""ReturnForm.aspx"">Return Requests</A></li>"
                //End If


            }

        }

    }
}