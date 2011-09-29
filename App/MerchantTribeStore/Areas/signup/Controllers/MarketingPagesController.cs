using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MerchantTribe.Commerce;

namespace MerchantTribeStore.Areas.signup.Controllers
{
    public class MarketingPagesController : BaseSignupController
    {        
        //GET: /signup
        public ActionResult Pricing()
        {            
            bool IsPayPalLead = false;
            if (MerchantTribe.Commerce.SessionManager.GetCookieString("PayPalLead") != string.Empty)
            {
                IsPayPalLead = true;
            }

            ViewBag.IsPayPalLead = IsPayPalLead;            
            return View();
        }

        //GET: /signup/about
        public ActionResult About()
        {
            return View();
        }

        //GET: /signup/desgin
        public ActionResult Design()
        {
            return View();
        }


        public ActionResult Features()
        {
            return View();
        }

        //GET: /signup/home
        public ActionResult Home()
        {
            return View();
        }

        //GET: /signup/paypal
        public ActionResult PayPalOffer()
        {
            SessionManager.SetCookieString("PayPalLead", "PayPalLead");
            return View();
        }

        //GET: /signup/themes
        public ActionResult Themes()
        {
            return View();
        }

        //GET: /signup/promote
        public ActionResult Promote()
        {
            return View();
        }

        //GET: /signup/secure
        public ActionResult Secure()
        {
            return View();
        }

        //GET: /signup/sell
        public ActionResult Sell()
        {
            return View();
        }
        
        //GET: /signup/tour
        public ActionResult Tour()
        {            
            return View();
        }
    }
}
