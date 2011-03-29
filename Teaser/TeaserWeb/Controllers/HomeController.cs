using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TeaserWeb.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {            
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(string email)
        {
            try
            {
                if (email.Trim().Length > 5)
                {
                    Models.EmailStorage.SaveEmail(email);
                }
            }
            catch (Exception ex)
            {
                // Swallow
            }
            return View();
        }

    }
}
