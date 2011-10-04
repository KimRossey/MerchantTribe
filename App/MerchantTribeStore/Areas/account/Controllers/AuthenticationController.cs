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
using MerchantTribeStore.Areas.account.Models;

namespace MerchantTribeStore.Areas.account.Controllers
{
    public class AuthenticationController : BaseStoreController
    {

        private void SignInSetup()
        {
            ViewBag.Title = SiteTerms.GetTerm(SiteTermIds.Login);
            
            List<BreadCrumbItem> extraCrumbs = new List<BreadCrumbItem>();
            extraCrumbs.Add(new BreadCrumbItem() { Name = ViewBag.Title });
            ViewBag.ExtraCrumbs = extraCrumbs;

            ViewBag.IsPrivateStore = MTApp.CurrentStore.Settings.IsPrivateStore;
            ViewBag.PrivateStoreMessage = SiteTerms.GetTerm(SiteTermIds.PrivateStoreNewUser);

            ViewBag.LoginButtonUrl = MTApp.ThemeManager().ButtonUrl("Login", Request.IsSecureConnection);
            ViewBag.CreateButtonUrl = MTApp.ThemeManager().ButtonUrl("createaccount", Request.IsSecureConnection);            
        }

        // GET: /account/signin
        public ActionResult SignIn()
        {
            SignInSetup();
            SignInViewModel model = new SignInViewModel();

            // Find email view cookie
            string uid = SessionManager.GetCookieString(WebAppSettings.CustomerIdCookieName);
            if (uid != string.Empty)
            {
                CustomerAccount u = MTApp.MembershipServices.Customers.Find(uid);
                if (u != null)
                {
                    model.Email = u.Email;
                }
            }

            return View(model);
        }

        // POST: /account/signin
        [ActionName("SignIn")]
        [HttpPost]
        public ActionResult SignInPost(SignInViewModel posted)
        {
            SignInSetup();

            if (Request.QueryString["mode"] != null)
            {
                posted.Mode = Request.QueryString["mode"];
            }

            if (ValidateLoginModel(posted, false))
            {
                string errorMessage = string.Empty;
                string userId = string.Empty;
                if (MTApp.MembershipServices.LoginCustomer(posted.Email.Trim(),
                                        posted.Password.Trim(),
                                        ref errorMessage,
                                        this.Request.RequestContext.HttpContext,
                                        ref userId))
                {
                    MerchantTribe.Commerce.Orders.Order cart = SessionManager.CurrentShoppingCart(MTApp.OrderServices);
                    if (cart != null && !string.IsNullOrEmpty(cart.bvin))
                    {
                        cart.UserEmail = posted.Email.Trim();
                        cart.UserID = userId;
                        MTApp.CalculateOrderAndSave(cart);
                        SessionManager.SaveOrderCookies(cart);
                    }

                    // if we got here from checkout, return to checkout
                    if (posted.Mode.Trim().ToLowerInvariant() == "checkout")
                    {
                        return Redirect("~/checkout");
                    }
                    // otherwise send to account home
                    return Redirect("~/account");
                }
                else
                {
                    string errorMessage2 = string.Empty;
                    // Failed to Login as Customer, Try admin account
                    if (MTApp.AccountServices.LoginAdminUser(posted.Email.Trim(), posted.Password.Trim(),
                                                                               ref errorMessage2, this.Request.RequestContext.HttpContext))
                    {
                        return Redirect("~/bvadmin");
                    }
                    this.FlashWarning(errorMessage);
                }
            }
            
            return View(posted);
        }

        // POST: /account/authentication/create
        [HttpPost]
        public ActionResult CreateAccount(SignInViewModel posted)
        {
            SignInSetup();
            SignInViewModel model = new SignInViewModel();

            // bail out if this is a private store that doesn't allow registrations
            if (ViewBag.IsPrivateStore) return View("SignIn", model);

            // Process Requrest
            if (ValidateLoginModel(posted, true))
            {
                bool result = false;
              
                CustomerAccount u = new CustomerAccount();

                if (u != null)
                {
                    u.Email = posted.Email.Trim();
                    CreateUserStatus s = CreateUserStatus.None;
                    // Create new user
                    result = MTApp.MembershipServices.CreateCustomer(u, ref s, posted.Password.Trim());

                    if (result == false)
                    {
                        switch (s)
                        {
                            case CreateUserStatus.DuplicateUsername:                                
                                FlashWarning("That email already exists. Select another email or login to your current account.");
                                break;
                            default:                                
                                FlashWarning("Unable to save user. Unknown error.");
                                break;
                        }
                    }
                    else
                    {
                        // Update bvin field so that next save will call updated instead of create
                        MerchantTribe.Web.Cookies.SetCookieString(MerchantTribe.Commerce.WebAppSettings.CookieNameAuthenticationTokenCustomer(),
                                                                  u.Bvin,
                                                                  this.Request.RequestContext.HttpContext, false, new EventLog());
                        Redirect("~/account");
                    }
                }
            }
            return View("SignIn", model);
        }
        private bool ValidateLoginModel(SignInViewModel posted, bool isCreate)
        {
            bool result = true;
            if (posted == null) return false;
            if (!MerchantTribe.Web.Validation.EmailValidation.MeetsEmailFormatRequirements(posted.Email))
            {
                result = false;
                FlashWarning("Please enter a valid email address");
            }
            if (posted.Password.Trim().Length < WebAppSettings.PasswordMinimumLength)
            {
                result = false;
                FlashWarning("Password must be at least " + WebAppSettings.PasswordMinimumLength + " characters long.");
            }

            if (isCreate)
            {
                if (posted.PasswordConfirm != posted.Password)
                {
                    result = false;
                    FlashWarning("Passwords don't match. Please try again.");
                }
            }
            return result;
        }

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
