using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Content;
using MerchantTribe.Commerce.Membership;
using MerchantTribe.Commerce.Utilities;
using MerchantTribeStore.Models;
using MerchantTribeStore.Controllers.Shared;

namespace MerchantTribeStore.Areas.account.Controllers
{
    public class AuthenticationController : BaseStoreController
    {

        public ActionResult SignOut()
        {
            MTApp.MembershipServices.LogoutCustomer(Request.RequestContext.HttpContext);
            return Redirect("~/");
        }

        private void ForgotPasswordSetup(string email, string returnmode)
        {
            ViewBag.Title = SiteTerms.GetTerm(SiteTermIds.ForgotPassword);

            List<BreadCrumbItem> extraCrumbs = new List<BreadCrumbItem>();
            extraCrumbs.Add(new BreadCrumbItem() { Name = ViewBag.Title });
            ViewBag.ExtraCrumbs = extraCrumbs;

            ViewBag.SendButtonUrl = MTApp.ThemeManager().ButtonUrl("Submit", Request.IsSecureConnection);

            if (returnmode.Trim().ToLowerInvariant() == "1")
            {
                ViewBag.CloseUrl = Url.Content("~/checkout");
            }
            else
            {
                ViewBag.CloseUrl = Url.Content("~/signin");
            }

            ViewBag.Email = email;
            ViewBag.PostBackUrl = Url.Action("ForgotPassword", new {email = email, returnmode = returnmode});            
        }        
        public ActionResult ForgotPassword(string email, string returnmode)
        {
            ForgotPasswordSetup(email, returnmode);
            return View();
        }
        [HttpPost]
        [ActionName("ForgotPassword")]
        public ActionResult ForgotPasswordPost(string email, string returnmode)
        {
            ForgotPasswordSetup(email, returnmode);
            ViewBag.Email = email;
            SendReminder(email);
            return View();
        }
        private void SendReminder(string email)
        {
            if (!MerchantTribe.Web.Validation.EmailValidation.MeetsEmailFormatRequirements(email))
            {
                FlashWarning("Please enter a valid email address");
                return;
            }

            try
            {
                CustomerAccount thisUser = MTApp.MembershipServices.Customers.FindByEmail(email);
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
                                FlashFailure("Error while sending mail!");
                            }

                        }

                        FlashSuccess("Your new password has been sent by email.");
                    }
                    else
                    {
                        FlashFailure("An error occurred while trying to update password.");
                    }
                }
                else
                {
                    FlashFailure(SiteTerms.GetTerm(SiteTermIds.Username) + " not found.");
                }
            }


            catch (SystemMembershipUserException CreateEx)
            {
                switch (CreateEx.Status)
                {
                    case CreateUserStatus.UpdateFailed:
                        FlashFailure("Update to user account failed.");
                        break;
                    default:
                        FlashFailure(CreateEx.Message);
                        break;
                }
            }
        }
    }
}
