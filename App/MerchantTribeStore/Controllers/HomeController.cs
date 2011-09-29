using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MerchantTribe.Commerce;

namespace MerchantTribeStore.Controllers
{
    public class HomeController : Shared.BaseStoreController
    {
        // Redirects to the correct home URL without page name
        public ActionResult ToIndex()
        {
            string homeUrl = Url.Content("~");
            return RedirectPermanent(homeUrl);
        }

        //
        // GET: /Home/
        public ActionResult Index()
        {
            // Redirect to Sign up if we're multi-store
            // TODO - Change this to return the signup view instead
            if (!WebAppSettings.IsIndividualMode)
            {
                if (MTApp.CurrentStore.StoreName == "www")
                {
                    return Redirect("/signup/home");
                }
            }

            SessionManager.CategoryLastId = string.Empty;
            ViewBag.Title = MTApp.CurrentStore.Settings.FriendlyName;
            ViewBag.BodyClass = "store-home-page";

            return View();
        }

    }
}
