using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace MerchantTribeStore.Controllers
{

    // Used to rewrite URLs inside javascripts so that
    // stores in sub folders get the correct paths.
    public class JavascriptController : Controller
    {
        [OutputCache(VaryByHeader = "Host", VaryByParam = "filename", Duration = 150)]
        public ActionResult Index(string filename)
        {
            string sourcecode = string.Empty;

            switch (filename.Trim().ToLowerInvariant())
            {
                case "checkout.js":
                    sourcecode = LoadPhysical("~/Checkout.js");
                    break;              
            }

            string baseUrl = Url.Content("~");
            sourcecode = sourcecode.Replace("~/", baseUrl);

            Response.AddHeader("content-disposition", "attachment; filename=" + filename);
            return Content(sourcecode, "text/javascript");
        }

        private string LoadPhysical(string relativeName)
        {
            string result = string.Empty;
            

            string f = Server.MapPath(relativeName);
            if (System.IO.File.Exists(f))
            {
                result = System.IO.File.ReadAllText(f);
            }

            return result;
        }
    }
}

