using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BVCommerce.Filters;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Membership;
using MerchantTribe.Commerce.Contacts;

namespace BVCommerce.Areas.account.Controllers
{
    [CustomerSignedInFilter]
    public class ManagementController : BVCommerce.Controllers.Shared.BaseStoreController
    {
        private void PasswordSetup()
        {
            ViewBag.Title = "Change Password";
            ViewBag.MetaDescription = "Change Password | " + BVApp.CurrentStore.Settings.MetaDescription;
            ViewBag.MetaKeywords = BVApp.CurrentStore.Settings.MetaKeywords;
            ViewBag.BodyClass = "myaccountchangepasswordpage";
            ViewBag.SaveButtonUrl = BVApp.ThemeManager().ButtonUrl("submit", Request.IsSecureConnection);
        }
        // GET: /account/management/changepassword
        public ActionResult ChangePassword()
        {
            PasswordSetup();

            return View();
        }
        // POST: /account/management/changepassword
        [ActionName("ChangePassword")]
        [HttpPost]
        public ActionResult ChangePasswordPost(FormCollection posted)
        {
            PasswordSetup();

            string currentpassword = posted["currentpasswordfield"];
            string newpassword = posted["newpasswordfield"];

            CustomerAccount current = BVApp.MembershipServices.Customers.Find(SessionManager.GetCurrentUserId());
            if (current == null) return View();

            if (!BVApp.MembershipServices.DoPasswordsMatchForCustomer(currentpassword.Trim(), current))
            {
                FlashWarning("Check your current password and try again.");
                return View();                
            }

            try
            {
                if (newpassword.Trim().Length < WebAppSettings.PasswordMinimumLength)
                {
                    FlashWarning("Password must be at least " + WebAppSettings.PasswordMinimumLength + " characters long.");
                }
                else
                {
                    if (BVApp.MembershipServices.ChangePasswordForCustomer(current.Email, currentpassword.Trim(),
                                                        newpassword.Trim()))
                    {
                        FlashSuccess("Password Updated.");
                    }
                    else
                    {
                        FlashWarning("Password could not be updated.");
                    }
                }
            }
            catch (BVMembershipUserException cex)
            {
                switch (cex.Status)
                {
                    case CreateUserStatus.UpdateFailed:
                        FlashWarning("Couldn't Save Changes. " + cex.Message);
                        break;
                    case CreateUserStatus.InvalidPassword:
                        FlashWarning("Couldn't Save Changes. Check your password and try again.");
                        break;
                    default:
                        FlashFailure(cex.Message);
                        break;
                }
            }

            return View();
        }


        private void ChangeEmailSetup()
        {
            ViewBag.Title = "Change Email";
            ViewBag.MetaDescription = "Change Email | " + BVApp.CurrentStore.Settings.MetaDescription;
            ViewBag.MetaKeywords = BVApp.CurrentStore.Settings.MetaKeywords;
            ViewBag.BodyClass = "myaccountchangeemailpage";
            ViewBag.SaveButtonUrl = BVApp.ThemeManager().ButtonUrl("submit", Request.IsSecureConnection);
        }        
        // GET: /account/management/changeemail
        public ActionResult ChangeEmail()
        {
            ChangeEmailSetup();

            return View();
        }

        //
        // POST: /account/management/changeemail
        [ActionName("ChangeEmail")]
        [HttpPost]
        public ActionResult ChangeEmailPost(FormCollection posted)
        {
            ChangeEmailSetup();

            string currentpassword = posted["currentpasswordfield"];
            string newemail = posted["newemailfield"];
            ViewBag.NewEmail = newemail;

            CustomerAccount current = BVApp.MembershipServices.Customers.Find(SessionManager.GetCurrentUserId());
            if (current == null) return View();

            if (!BVApp.MembershipServices.DoPasswordsMatchForCustomer(currentpassword.Trim(), current))
            {
                FlashWarning("Your password was incorrect. Try Again.");
                return View();
            }

            try
            {
                if (!MerchantTribe.Web.Validation.EmailValidation.MeetsEmailFormatRequirements(newemail))
                {
                    FlashWarning("Please enter a valid new email address.");
                    return View();
                }
                
                string oldEmail = current.Email;
                BVApp.MembershipServices.UpdateCustomerEmail(current, newemail.Trim());

                List<MailingListSnapShot> lists = BVApp.ContactServices.MailingLists.FindAllPublicPaged(1, 1000);
                foreach (MailingListSnapShot m in lists)
                {
                    if (BVApp.ContactServices.MailingLists.CheckMembership(m.Id, oldEmail))
                    {
                        MailingList l = BVApp.ContactServices.MailingLists.Find(m.Id);
                        l.UpdateMemberEmail(oldEmail,newemail.Trim());
                    }
                }

                FlashSuccess("Email Address Changed");   
            }
            catch (BVMembershipUserException CreateEx)
            {
                switch (CreateEx.Status)
                {
                    case CreateUserStatus.DuplicateUsername:
                        FlashWarning("A user account with that email address already exists. Please select another email address.");
                        break;
                    case CreateUserStatus.InvalidPassword:
                        FlashWarning("Please check your password and try again.");
                        break;
                    case CreateUserStatus.UserNotFound:
                        FlashFailure("User account couldn't be located.");
                        break;
                }
            }

            return View();
        }

    }
}
